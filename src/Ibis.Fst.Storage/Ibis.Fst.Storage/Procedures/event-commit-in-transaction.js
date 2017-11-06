// Emits events (sharing the same partition ID - appropriate
// for the event aggragate) into the event store and updates the 
// stream header. 
function emiteventsintx(streamHeadDocID, partitionKey, partitionValue, eventDocuments) {
    var collection = __;
    var response = getContext().getResponse();

    var tryCreateDoc = function (coll, doc, onSuccess) {        
        var dcAccepted = coll.createDocument(coll.getSelfLink(), doc,
            function (error, doc) {
                onSuccess(!error);
            });

        if (!dcAccepted) {
            onSuccess(false);
        }
    }

    var ensureStreamHeader = function (resDocuments, onEnsure) {
        if (resDocuments != null && resDocuments.length === 1) {
            onEnsure(resDocuments[0]);
        } else {
            var sh = JSON.parse(' { "id": "' + streamHeadDocID + '", "' + partitionKey + '": "' + partitionValue + '", "eventCount" : 0, "isStreamHead" : true  } ');
            var shDocAccepted = collection.createDocument(collection.getSelfLink(), sh,
                function (error, ndoc) {
                    if (error) { throw "Unable to create stream head!"; }
                    else { onEnsure(ndoc); }
                });

            if (!shDocAccepted) {
                throw "Unable to submit create stread head request!";
            }
        }
    }


    var q = 'SELECT * FROM Projects p where p.id = "' + streamHeadDocID + '"';

    var accepted = collection.queryDocuments(collection.getSelfLink(), q, {},
        function (error, documents, responseOptions) {
            ensureStreamHeader(documents, function (streamHeadDoc) {
                var sequence = streamHeadDoc.eventCount;
                streamHeadDoc.eventCount = streamHeadDoc.eventCount + eventDocuments.length;

                for (var count = 0; count < eventDocuments.length; ++count) {
                    eventDocuments[count].eventSequence = (++sequence);
                }
                var shAccept =
                    collection.replaceDocument(streamHeadDoc._self, streamHeadDoc,
                        function (shError, replacedHeaderDoc) {
                            if (shError) throw "Unable to update stream header!";

                            var docIterator = 0;
                            var onAfterAdded = function (success) {
                                if (!success) {
                                    throw "Unable to update document (" + docIterator + ")!";
                                } else {
                                    docIterator = docIterator + 1;
                                    if (docIterator < eventDocuments.length) {
                                        tryCreateDoc(collection, eventDocuments[docIterator], onAfterAdded);
                                    } else {
                                        response.setBody(true);
                                    }
                                }
                            }
                            tryCreateDoc(collection, eventDocuments[docIterator], onAfterAdded);

                        });
                if (!shAccept) {
                    throw "Unable to update stream header!"
                }
            });
        });

    if (!accepted) throw new Error('The query was not accepted by the server.');
}

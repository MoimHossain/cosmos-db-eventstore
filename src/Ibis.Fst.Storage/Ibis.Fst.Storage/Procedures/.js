// SAMPLE STORED PROCEDURE
function emiteventsintx(prefix, eventDocuments) {
    var tryCreateDoc = function (coll, doc, eventId, onSuccess) {
        doc.eventId = eventId;
        var dcAccepted = coll.createDocument(coll.getSelfLink(), doc,
            function (error, doc) {
                onSuccess(!error);
            });

        if (!dcAccepted) {
            onSuccess(false);
        }
    }

    var collection = __;
    var streamHeadDocID = 'event-stream-head';
    var q = 'SELECT * FROM Projects p where p.id = "' + streamHeadDocID + '"';
    var accepted = collection.queryDocuments(collection.getSelfLink(), q, {},
        function (error, documents, responseOptions) {
            if (documents.length != 1) throw "Unable to find stream head!";
            var streamHeadDoc = documents[0];
            for (var count = 0; count < documents.length; ++count) {
                streamHeadDoc.eventCount = streamHeadDoc.eventCount + 1;
                documents[count].eventId = streamHeadDoc.eventCount;
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
                                    tryCreateDoc(collection, eventDocuments[docIterator], streamHeadDoc.eventCount + docIterator, onAfterAdded);
                                }
                            }
                        }
                        tryCreateDoc(collection, eventDocuments[docIterator], streamHeadDoc.eventCount + docIterator, onAfterAdded);

                    });
            if (!shAccept) {
                throw "Unable to update stream header!"
            }
        });

    if (!accepted) throw new Error('The query was not accepted by the server.');
}

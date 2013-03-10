var specs = (function() {

    var fetchResponse = function (method, url, callback) {
        var response = undefined;

        $.ajax({
            type: method,
            url: url,
            success: function(res) {
                response = res;
            }
        });

        waitsFor(function() {
            return response !== undefined;
        }, 1000);

        runs(function() {
            callback(response);
        });
    };

    var magic = function() {
        var description = this.description,
            parser = new RegExp(/should respond to (\w+) "([\w-\/\?=]+)" with "(.+?)"/i),
            match = parser.exec(description),
            method, url, expectedResponse;

        // Validate the format of the spec's description.
        if (!match || match.length !== 4) {
            throw new Error('The spec description is not valid.\n' +
                'Expected: should respond to METHOD "/url" with "response"');
        }

        // Parse the relevant params from the description.
        method = match[1];
        url = match[2];
        expectedResponse = match[3];

        // Perform the assertion.
        fetchResponse(method, url, function(response) {
            expect(response).toEqual(expectedResponse);
        });
    };

    return {
        magic: magic
    };
}());

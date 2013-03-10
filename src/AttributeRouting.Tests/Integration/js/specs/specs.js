var specs = (function() {

    function fetchResponse(method, url, callback) {
        var response = undefined;

        $.ajax({
            type: method,
            url: url,
            success: function(res) {
                response = res;
            },
            error: function(res) {
                response = res.status + '';
            }
        });

        waitsFor(function() {
            return response !== undefined;
        }, 1000);

        runs(function() {
            callback(response);
        });
    }

    function parseSpecDescription(description) {
        var parser = new RegExp(/should respond to (\w+) "([^\"]+)" with "?([^\"]+|\d+)"?/i),
            match = parser.exec(description);

        // Validate the format of the spec's description.
        if (!match || match.length !== 4) {
            throw new Error('The spec description is not valid.\n' +
                'Expected: should respond to METHOD "/url" with {"response" or STATUS}');
        }

        // Parse the relevant params from the description.
        return {
            method: match[1],
            url: match[2],
            expectedResponse: match[3]
        };
    }
    
    function respondsWith() {
        var params = parseSpecDescription(this.description);

        // Ensure we get the expected response.
        fetchResponse(params.method, params.url, function (response) {
            expect(response).toEqualValue(params.expectedResponse);
        });
    }

    // Export shared specs:
    return {
        respondsWith: respondsWith
    };
}());

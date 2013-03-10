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

    function respondsWith() {
        var parser = new RegExp(/responds to (\w+) "([^\"]+)" with "?([^\"]+|\d+)"?/i),
            match = parser.exec(this.description),
            method, url, expectedResponse;

        // Validate the format of the spec's description.
        if (!match || match.length !== 4) {
            throw new Error('The spec description is not valid.\n' +
                'Expected: responds to METHOD "/url" with {"response" or STATUS}');
        }

        // Parse the relevant params from the description.
        method = match[1];
        url = match[2];
        expectedResponse = match[3];

        // Ensure we get the expected response.
        fetchResponse(method, url, function (response) {
            expect(response).toEqualValue(expectedResponse);
        });
    }

    // Export shared specs:
    return {
        respondsWith: respondsWith
    };
}());

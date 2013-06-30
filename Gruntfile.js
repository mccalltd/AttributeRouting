module.exports = function (grunt) {
    var fs = require('fs'),
        wrench = require('wrench'),

        directories = {
            build: __dirname + '\\build',
            out: __dirname + '\\build\\bin',
            src: __dirname + '\\src'
        }
    ;

    // Project configuration
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        msbuild: {
            release: {
                src: ['src\\AttributeRouting.sln'],
                options: {
                    projectConfigurations: 'Release',
                    targets: ['Clean', 'Rebuild'],
                    stdout: true,
                    buildParameters: {
                        WarningLevel: 2,
                        OutputPath: directories.out
                    },
                    verbosity: 'minimal'
                }
            }
        }
    });

    // Load dependencies
    grunt.loadNpmTasks('grunt-msbuild');

    // Custom tasks
    grunt.registerTask('clean', 'Cleans the build directory.', function() {
        var done = this.async();

        console.log('cleaning', directories.build);

        wrench.rmdirRecursive(directories.build, function() {
            fs.mkdir(directories.build, done);
        });
    });

    // Tasks
    grunt.registerTask('default', ['clean', 'msbuild']);
};
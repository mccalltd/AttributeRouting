module.exports = function (grunt) {
  var fs = require('fs')
    , wrench = require('wrench')

    , DIRECTORIES = {
      build: __dirname + '\\build',
      out: __dirname + '\\build\\bin',
      src: __dirname + '\\src'
    }
  ;

  //======================================
  // Project configuration
  //======================================

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
            OutputPath: DIRECTORIES.out
          },
          verbosity: 'minimal'
        }
      }
    },
    shell: {
      test: {
        command: [
          'tools\\nunit\\nunit-console-x86.exe',
          DIRECTORIES.out + '\\AttributeRouting.Specs.dll',
          '/work:' + DIRECTORIES.build,
          '/out:TestResults.txt',
          '/result:TestResults.xml',
          '/nologo',
          '/nodots'
        ].join(' '),
        options: {
          stdout: true
        }
      }
    }
  });

  //======================================
  // Load dependencies
  //======================================

  grunt.loadNpmTasks('grunt-msbuild');
  grunt.loadNpmTasks('grunt-shell');

  //======================================
  // Custom tasks
  //======================================

  grunt.registerTask('clean', 'Cleans the build directory.', function () {
    var done = this.async();

    console.log('cleaning', DIRECTORIES.build);

    wrench.rmdirRecursive(DIRECTORIES.build, function () {
      fs.mkdir(DIRECTORIES.build, done);
    });
  });

  //======================================
  // Tasks
  //======================================

  grunt.registerTask('default', ['clean', 'msbuild', 'shell:test']);
};
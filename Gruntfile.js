module.exports = function (grunt) {
  var fs = require('fs')
    , prompt = require('prompt')
    , wrench = require('wrench')

    , directories = {
      build: __dirname + '\\build',
      out: __dirname + '\\build\\bin',
      src: __dirname + '\\src'
    }
    , files = {
      assemblyInfo: directories.src + '\\SharedAssemblyInfo.cs'
    }
    , version
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
            OutputPath: directories.out
          },
          verbosity: 'minimal'
        }
      }
    },
    
    shell: {
      test: {
        command: [
          'tools\\nunit\\nunit-console-x86.exe',
          directories.out + '\\AttributeRouting.Specs.dll',
          '/work:' + directories.build,
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

  grunt.registerTask('prompt', 'Prompts for required build params.', function () {
    var done = this.async();
    var defaultVersion;
    var schema;

    fs.readFile(files.assemblyInfo, function(err, data) {
      if (err) throw err;

      defaultVersion = /AssemblyInformationalVersion\("(.*)"\)/.exec(data.toString())[1];

      prompt.start();

      schema = {
        properties: {
          version: {
            default: defaultVersion,
            pattern: /^\d+\.\d+(\.\d+){0,2}(-\S)?$/,
            message: 'version must be a valid semantic version.',
            required: true
          }
        }
      };
      
      prompt.get(schema, function (err, result) {
        if (err) throw err;
        version = result.version;
        done();
      });
    });
  });
  
  grunt.registerTask('clean', 'Cleans the build directory.', function () {
    var done = this.async();

    wrench.rmdirRecursive(directories.build, function () {
      fs.mkdir(directories.build, done);
    });
  });

  grunt.registerTask('assemblyInfo', "Creates the shared AssemblyInfo.cs file.", function () {
    grunt.task.requires('prompt');
    
    var done = this.async();
    var nonPrereleaseVersion = version.split('-')[0];
    var fileContent = [
      'using System;',
      'using System.Reflection;',
      'using System.Runtime.InteropServices;',
      '',
      '[assembly: ComVisible(false)]',
      '[assembly: AssemblyCompany("")]',
      '[assembly: AssemblyProduct("AttributeRouting")]',
      '[assembly: AssemblyCopyright("Copyright 2010-' + new Date().getFullYear() + ' Tim McCall")]',
      '[assembly: AssemblyTrademark("")]',
      '[assembly: AssemblyVersion("'+ nonPrereleaseVersion + '")]',
      '[assembly: AssemblyFileVersion("'+ nonPrereleaseVersion + '")]',
      '[assembly: AssemblyInformationalVersion("'+ version + '")]',
      '[assembly: AssemblyConfiguration("Release")]'
    ].join('\r\n');
    
    fs.writeFile(files.assemblyInfo, fileContent, done);
  });

  grunt.registerTask('nugetPack', 'Creates nuget packages from build output.', function() {

  });
  
  //======================================
  // Tasks
  //======================================

  grunt.registerTask('default', ['build']);
  
  grunt.registerTask('build', ['prompt', 'clean', 'msbuild', 'assemblyInfo', 'shell:test']);

  grunt.registerTask('package', ['build', 'nugetPack']);
};
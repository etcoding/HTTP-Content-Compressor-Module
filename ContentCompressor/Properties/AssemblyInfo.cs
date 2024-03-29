﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Web;
using ET.ContentCompressorModule;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("ContentCompressor")]
[assembly: AssemblyDescription("This is simple drop-and-forget, configuration-less (optional) http module that will handle compression of javascript and css, both in files and embedded on html page.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Etcoding")]
[assembly: AssemblyProduct("ContentCompressor")]
[assembly: AssemblyCopyright("Copyright © Evgeni Tsarovski 2012")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("14f730e7-84fc-45ce-b69b-c9339570c0ef")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.1")]
[assembly: AssemblyFileVersion("1.0.0.1")]
[assembly: PreApplicationStartMethod(typeof(CompressorModule), "RegisterSelf")]
# TypeWalker

With TypeScript, Microsoft introduced a nice, strongly-typed way to develop a variant of JavaScript. It's been written to be nicely agnostic to other languages -- it'll run for node.js server-side apps just as easily as integrating with your C# MVC projects. However, for complex C# apps, it's really useful to have your TypeScript and your C# in lockstep. 

For instance, let's say you have an MVC controller returning a list of person records;

    [HttpGet]
    public ActionResult GetPeople() 
    {
    	PersonDto[] people = personRepo.GetAll(l);
        return Json(people, JsonRequestBehavior.AllowGet);
    }

So far so good. You probably consume it in JavaScript like so;

    $.getJson('/People/GetPeople', function(data) {
    	// data will look like; 
    	//    [
        //        { "firstName": "alice", "id": 1 },
        //        { "firstName": "bob", "id": 2 },
     	//    ]
    });

But in TypeScript we want to strongly type it, so we want to do something more like;

    $.getJson('/People/GetPeople', function(data: PersonDto[]) {
    	...
    });

where you can now see the `PersonDto[]` type annotation. This gives you strong typing throughout the function. So somehow I need to write a typescript interface;

    interface PersonDto {
    	firstName: string;
    	id: number;
    }

This is fine, but you've already got a PersonDto.cs file somewhere in your C# file, and there is no benefit in you coding both, at the risk of violating DRY, and at the risk of things going out-of-date over time. 

Also, I like [Knockout.js](http://knockoutjs.com), and especially the mapping plugin, which converts POJOs into bindable objects. But these have different interfaces from the original object -- a `name` property becomes a `name()` getter function and a `name(value)` setter function. This is fairly predicable, so to get TypeScript support you would need to add a third description of the class;

    module KnockoutVersion {
        interface PersonDto {
            firstName(): string;
            firstName(value:string): void;
            id(): number;
            id(value:number): void;
        }
    }

So that's three descriptions of the same basic type. Urgh!

TypeWalker is a project which generates your TypeScript definitions from your C# types. This spares you the effort of keeping things in sync, and offers some stronger guarantees that your JavaScript will be less likely to contain the mistakes that creep in when people add or remove properties to classes or otherwise change the signature of a type.

At the moment it operates using the command line. You should be able to use it now by calling it manually. I am currently working at building it as a NuGet package which will extend a web application project and automatically build your TypeScript types at build time.

# Command Line Operation

    TypeWalker 
        /configFile=c:\src\mysite\AssembliesToOutput.txt 
        /language=KnockoutMapping 
        /knockoutPrefix=KOGen 
        /outputFile=c:\src\mysite\scripts\typings\mysite.d.ts

`configFile` is a text file containing lines formatted like so;

    MyClassLibrary::My.Namespace.To.Export

So, pairs of assembly names (without .dll extension) and a namespace within that class library to export.

`language` is currently one of `KnockoutMapping` or `TypeScript`. You can call the app twice if you want both. Over time, I'll make the languages pluggable, but right now they're fixed.

`knockoutPrefix` is a namespace prefix for Knockout-generated files. This lets you generate `MyModule.Person` for your POJOs and `KO.MyModule.Person` for your knockout versions.

`outputFile` is just the file where the file should be written. Usually a `.d.ts` TypeScript definition file.


# NuGet Integration

There's rudimentary NuGet support;

    Install-Package TypeWalker -Pre

Run this in your web app. This will install the binary and modify the .csproj file with the appropriate MSBuild targets. You now need to save the project, close and re-open the solution.

Next, you need to write a file containing the assembly names and namespaces to export -- see above under the `configFile` documentation. Add this file to the project, then edit the properties of the file. Change the Build Action to `GenerateTypeScriptResources`.

Save, possibly close and re-open again, then start building. It'll generate your output, switching the extension to `.d.ts`. For example;

    Scripts/TypeWalker.txt
    Scripts/TypeWalker.d.ts

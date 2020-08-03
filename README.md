# TravelTracker
An Asp.net site that lets you track places you've visited an placed you plan to visit around the world. <br/>

### Running the web app
To run the web app:
- clone the project and open TravelTracker.sln in Visual Studio.
- Insure you have Sql Server Installed on your machine.
- From inside Visual studio, open the NuGet Package Manager Console by clicking Tools > NuGet Package Manager > Package Manager Console.
- On the Package Manager Console, type: `update-database`, this will initialize the database. <br>

From here the app will run but the maps api wont work until you create and use an api key. Here's a quick way to create a key: https://developers.google.com/maps/documentation/javascript/get-api-key. Don't Worry about restricting your key. Once you have your api key:
- modify the file TravelTracker\appsettings.json by replacing `ADD_YOUR_API_KEY_HERE` with your key
- finally, from Visual Studio, build and run the app. The site should launch on your web browser upon completion.


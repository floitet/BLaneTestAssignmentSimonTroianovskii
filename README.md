# BallastLaneTestAssignment

## Thought process
- As the assignment requires following clean Architecture principles, I decided to start with building up the project structure.
I wanted to get the project to the point where it would look more or less production-like, so I did not want to start from scratch and looked for the appropriate template. I found one that looked very similar to the structure we use at my current workplace (we follow the Clean Architecture at my current place). The template covered a lot of boilerplate for me, but it was just the very beginning
- As it was requested to not use EF Core and the template heavily relied on it, I had to refactor it a lot (I left EF Core just for the Identity module, as it requests it out-of-the-box). For all the custom logic I implemented, I refactor completely, providing Dapper services to handle communication with DB. I have implemented repository + unit of work patterns with Dapper to ensure a similar work experience as if I used Ef Core, except for now the queries to db are explicit, but some manual heavy lifting needs to be done to make things work. The implementation I ended up with is far from perfect but seems to be a solid foundation for the next steps.
- I have also created a stand-alone project for Migrations, that would not depend on EF Core (went for FluentMigrator). You just give a connection string and it will do the job. Possibly, the DB could be migrated from the main API with the same code, but in this case, I think a stand-alone migration project is a more viable option. 
- Having abstractions over Dapper set up with basic operations available I started working on tests and business logic, making sure they were in line on each step.
- Along with Unit tests I aligned the integrational tests that are supposed to work with the real DB, as when it comes to raw SQL, it is especially crucial to test against a real DB. In general, I find it very important to have the real DB testing setup available in the project.
- The template came out with some kind of UI with Angular, which I tweaked to be in line with my use cases, but it's far from perfect, and requires a lot more work to be anywhere close to a production-like product. With this being said, it serves the purpose of showcasing API, registering and logging in a user, and serving as one of the entry points to the project exploration.

## System specs
The system I have come up with is built to cover the points from the assignment requirements. 
- It has a user endpoint to register users, generate tokens, etc. The corresponding process can be handled through the UI.
- It has policies set up for various user roles, and some of the business logic paths are not available for regular users.
- It allows authorized users to work on the list of prescriptions, which can contain prescription items (drugs presumably prescribed to the client)
- It allows updating lists, updating items, etc etc. With this being said, the UI client is not very well polished and does not support all the operations.

## Conclusion
With that being said, I leave the rest of the story to be told by the code itself. 
Please let me know of your comments if any! 
  

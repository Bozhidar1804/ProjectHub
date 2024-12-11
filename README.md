# ProjectHub Web Project

## Overview & Concept

This is my web application for SoftUni ASP.NET final defence.
Using ASP.NET MVC, ProjectHub is designed for creating and managing Projects, including various functionalities. Each User can apply for joining a Project through a Candidature. The Moderator(ProjectCreator) can review all pending Candidatures and Approve or Deny them.
Each Candidature contains answers to the questions that everyone should answer in order to apply for the Project.
The ProjectCreator creates Milestones for the Project, which contain Tasks. Tasks are assigned to a specific User, who is a part of the Project. The User has to Complete the Task in order for the team to progress with the Milestones.
Every Member of the Project can add Comments to a Task. The Comments can be Upvoted or Downvoted. Some of the Actions made to a Task are logged in the ActivityLog entity.

- The site has Responsive layout for different devices.

## Database Diagram

![DBDiagram1](https://github.com/user-attachments/assets/2c083716-3700-4f6b-8038-0e3db08cb317)
![DBDiagram2](https://github.com/user-attachments/assets/fbbe0ab6-59f4-451d-b25a-26a645318882)

## UserRoles:

### User Role:

Users can view all projects, apply to the ones that they aren't a part of, and view every project's details. They can create candidatures whenever applying for a project. A user can view every task that is assigned to him and complete it. Also, every user can add comments to every task in their project. A user can either upvote or downvote a comment.

### Moderator (ProjectCreator) Role:

The Moderator has every functionality of the User. He can also create a project. As a ProjectCreator, he can approve or deny candidatures. He can define the milestones of the project. The Moderator can create and assign tasks to members of the project. He can delete a project or edit it. The edit contains an option for reassigning a task from one user to another.

### Admin Role:

The Admin can manage every User's role, promoting or demoting to a Moderator. He can delete every project in the application. He can see activities and actions made to every task. The Admin has a view which is displaying some statistics about the whole application.

## Some information about the functionality:

- Unauthenticated users can see only the Home page of the application, displaying the purpose of the platform and the questions for the candidatures.
- If a User has applied to a Project and his Candidature is pending, he can't apply to the Project again.
- Projects are visualised in 2 groups: 1. Projects the User is a part of; 2. Projects the User isn't a part of and can apply to;
- Candidatures are visualied in 3 groups: 1. Pending; 2. Approved; 3. Denied;
- In the end of every Action, the User is being redirected to the most proper and appropriate page based on the logic of Action.
- Before starting to create Tasks, the ProjectCreator has to define how much Milestones the Project has. The MaxMilestones value can be changed in the Edit of the Project.
- User's Awaiting Tasks are ordered 1. By their Project, 2. By the Milestone they are linked to.
- A User can either Upvote or Downvote a Comment.
- The following actions made to a task are logged: 1. Creation of the Task; 2. ReAssignment to a different User; 3. Completion of the task; 4. The adding of a Comment to the Task. These logs are displayed in the Admin Dashboard.
- Milestone Progress is calculated dinamically.
- The application contains Pagination functionality in Admin/Management/UserRoles.
- The application contains Filtering functionality in Admin/Management/Projects.

## Seeding information:

- The Data Seeding happens in the Program.cs. Entities in each Seed method are added to the database if the table is empty. Seed all entities at once when all tables are empty.

### USERS:

Admin - admin@gmail.com - admin123 <br />

Moderator1 - moderator1@gmail.com - mod123 <br />
Moderator2 - moderator2@gmail.com - mod123 <br />

User1 - user1@gmail.com - user123 <br />
User2 - user2@gmail.com - user123 <br />
User3 - user3@gmail.com - user123 <br />

### PROJECTS:

Moderator1 - has 3 projects; <br />
Moderator2 - has 2 projects; <br />

Software Development Project - has 2 members - Mod1, User2; <br />
Construction Project - has 1 member - Mod2; <br />
Event Planning - has 2 members - Mod1, User1; <br />
Marketing Campaign - has 1 member - Mod2; <br />
Research Project - has 2 members - Mod1, User3; <br />

### CANDIDATURES:

User1 - has 1 approved candidature, 1 pending candidature; <br />
User2 - has 1 approved candidature, 1 pending candidature; <br />
User3 - has 1 approved candidature, 1 denied candidature; <br />

Moderator2 - has 2 Candidatures To Review. <br />

## Azure Deployment:

- I tried to deploy the application using Azure, but the domain displays HTTP Error 500.30 - ASP.NET Core app failed to start. I have done the migration script into the azure database, but the error still occurrs.

## Notes:

- The Registering of the Services happens in 2 different methods: the services that are being used only by the Admin are registered in a separate method.
- SoftDetele is being used in the Application.
- secrets.json:
  {
  "Identity": {
  "Password": {
  "RequireDigits": true,
  "RequireLowercase": false,
  "RequireUppercase": false,
  "RequireNonAlphanumeric": false,
  "RequiredLength": 3,
  "RequiredUniqueChars": 1
  },
  "SignIn": {
  "RequireConfirmedAccount": false,
  "RequireConfirmedEmail": false,
  "RequireConfirmedPhoneNumber": false
  },
  "User": {
  "RequireUniqueEmail": true
  }
  }
  }

## What I would improve and change in the Application:

- I would fix the ambiguous name reference for the Task entity - it matches System.Threading.Tasks.Task;
- I should have made Git commits more frequent and detailed;
- I would add filtering options around the project;
- I would add more images around the application;
- I would implement Repository Design Patern;
- Implement functionality related to the StartDate and EndDate of the projects.

## Technologies used:

- C#
- ASP.NET with MVC pattern
- Entity Framework core
- Microsoft SQL Server
- JavaScript
- Html
- Css
- NUnit
- JSON

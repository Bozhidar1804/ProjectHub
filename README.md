# ProjectHub Web Project

## Overview & Concept

This is my web application for SoftUni ASP.NET final defence.
Using ASP.NET MVC, ProjectHub is designed for creating and managing Projects, including various functionalities. Each User can apply for joining a Project through a Candidature. The Moderator(ProjectCreator) can review all pending Candidatures and Approve or Deny them.
Each Candidature contains answers to the questions that everyone should answer in order to apply for the Project.
The ProjectCreator creates Milestones for the Project, which contain Tasks. Tasks are assigned to a specific User, who is a part of the Project. The User has to Complete the Task in order for the team to progress with the Milestones.
Every Member of the Project can add Comments to a Task. The Comments can be Upvoted or Downvoted. Some of the Actions made to a Task are logged in the ActivityLog entity.

## Database Diagram

<img src="https://prnt.sc/1SZhcgm9nVEB" alt="DBDiagram1" width="500"/>

![image](https://prnt.sc/1SZhcgm9nVEB)
![image](https://prnt.sc/66qlQkOVreSw)

## UserRoles:

### User Role:

Users can view all projects, apply to the ones that they aren't a part of, and view every project's details. They can create candidatures whenever applying for a project. A user can view every task that is assigned to him and complete it. Also, every user can add comments to every task in their project. A user can either upvote or downvote a comment.

### Moderator (ProjectCreator) Role:

The Moderator has every functionality of the User. He can also create a project. As a ProjectCreator, he can approve or deny candidatures. He can define the milestones of the project. The Moderator can create and assign tasks to members of the project. He can delete a project or edit it. The edit contains an option for reassigning a task from one user to another.

### Admin Role:

The Admin can manage every User's role, promoting or demoting to a Moderator. He can delete every project in the application. He can see activities and actions made to every task. The Admin has a view which is displaying some statistics about the whole application.

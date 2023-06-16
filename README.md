# C# Forum Project
### Description

Forum project is an online platform template built using ASP .NET that enables users to register, create own posts, react and reply to other useres posts and exchange messages. All wrapped up in a raw but user friendly interface.  
Key features:  
* <dt> User Registration and Authentication </dt> Users can create new accounts by providing their details and securely authenticate themselves to access the forum. This feature ensures that only registered users can participate in discussions.  
* <dt> Messages </dt> Users can exchange messages visible only to them.  
* <dt> Search Functionality </dt> The forum incorporates a search feature that enables users to find specific posts based on title and tags. This helps users discover relevant content and retrieve information quickly.  
* <dt> Reactions system </dt> Users can upvote or downvote threads and messages to indicate their agreement or disagreement with the content.   

### Project Structure
<pre>
Forum   
  ├── Controllers                 # APP CONTROLLERS  
  |        ├── AdminController.cs  
  |        ├── AuthController.cs    
  |        ├── CommentController.cs  
  |        ├── HomeController.cs  
  |        ├── MessageController.cs  
  |        ├── PostController.cs  
  |        ├── TagController.cs  
  |        └── UserController.cs  
  ├────── Data                    # HANDLING CONNECTION WITH DATABASE  
  |        └── ForumDbContext.cs  
  ├────── Models                  # APP MODELS  
  |        ├── Comment.cs  
  |        ├── Message.cs  
  |        ├── Post.cs  
  |        ├── Tag.cs  
  |        └── User.cs    
  ├────── Properties              # SOME SETTINGS  
  |        └── lauchSettings.json  
  ├────── Views                   # APP VIEWS    
  |        ├── Admin    
  |        ├── Auth    
  |        ├── Comment    
  |        ├── Home     
  |        ├── Message   
  |        ├── Post   
  |        ├── Shared   
  |        ├── Tag  
  |        └── User  
  ├────── Program.cs            # APP ENTRY  
  ├────── appsettings.Development.json  
  ├────── appsettings.json  
  └────── forum.csproj  
  </pre>
  
  
### Database schema
Project uses simple sqlite3 database  
![](https://github.com/witek3100/C--Projekt/blob/main/assets/database_schema.png)

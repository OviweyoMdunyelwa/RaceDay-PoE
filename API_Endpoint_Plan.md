## API Endpoint Plan

The following API endpoint plan outlines the proposed RESTful API for the RaceDay event management system. The endpoints are designed around the main system resources, including users, events, categories, enrolments, results, routes and weather information. Each endpoint identifies the HTTP method, route, responsible user role, purpose, request information and expected response codes. This plan provides a clear structure for how the frontend and backend components will communicate and how different users will interact with the RaceDay system.


API END POIN PLAN

HTTP Method 	Route 	Desrcption 	Role Required 	Request Body	Expected Response 
Authentication
POST	/api/auth/register	Register a new user account as Organizer or a Participant	none	{ fisrtName ,lastName ,email .passwords , role }	201 Created – user registered 
400 Bad Request-missing/invalid fields
409 Conflict-email already registered
POST	/api/auth/login	Authenticates a user and returns a JWT access token	none	{ email , password }	200 OK-token returned 
401 Unauthorized-invalid credentials 


HTTP Method	Route	Desrcption	Role Required	Request Body	Expected Response
User Profile
GET	/api/users/profile	Return the profile details of the currently logged in user 	any	None	200 OK-profile returned 
400 Bad Request-invalid fields
401 Unauthorized 
PUT	/api/users/profile	Updates the logged in user own profile details 	any	[firstName ,lastName ,phone , province }	200 OK -profile updated 
400 Bad request-invalid fields 
401 Unauthorized 
GET	/api/users/profile/history	Returns a Participant personal performance history across all past events entered 	Participant	None	200 OK-list of past results returned 
401 Unauthorized
404 Not found -no history yet 


HTTP Method	Route	Desrcption	Role Required	Request Body	Expected Response
Events
GET	/api/events	List all upcoming events so Participants can browse them	None	None	200 OK-list of events returned
GET	/api/events/{id}	Returns full details of a single events including route and location information 	None	None	200 OK -events created 
404 Not Found-event does not exist
POST	/api/events	Creates a new event	Organizer	{name, description ,date, location , routeMapUrl  }	201 Created-event created 
400 Bad Request-Invalid fields
401 Unauthorized 
403 Forbidden-not an Organiser
PUT	api/events/{id}	Updates an existing event owned by the logged in Organizer 	Organizer	{name, description ,date, location , routeMapUrl  }	200 OK-event uploaded
400 Bad Request-invalid fields
403 Forbidden-not the event owner
404 Not Found
DELETE 	api/events/{id}	Cancels/delete an event owned by the logged  in Organizer 	Organizer	None	200 OK-event deleted 
403 Forbidden-not the event owner
404 Not Found 
GET	api/events/{id}/weather	Restrivs the race day weather forecast for an event date and location .	Any	None	200 OK -forecast returned 
404 Not Found-event does not exist 
503 Service Unavailable -weather provider failed 


HTTP Method	Route	Desrcption	Role Required	Request Body	Expected Response
Categories
GET	api/events/{id}/categories	Lists all categories (e.g 5km , 10km , Half Marathon ) available for an event.	None	None	200 OK-list of categories returned 
404 Not Found-event does not exist 
POST	api/events/{id}/categories	Adds a new category to an event.	Organizer	{ name ,distanceKm,entryFee}	201 Created -category created 
400 Bad Request -invalid fields 
403 Forbidden-not the event owner 
404 Not Found 
PUT	/api/categories/{id}	Updates an existing category 	Organizer	{ name ,distanceKm,entryFee}	200 OK-category updated 
400 Bad Request 
403 Forbidden
404 Not Found 
DELETE	/api/categories/{id}	Deletes a category from an event 	Organizer	None	200 OK-category created 
403 Forbidden
404 Not Found



HTTP Method	Route	Desrcption	Role Required	Request Body	Expected Response
Events Enrolments
POST	api/events/{id}/enrol	Enters the logged in Participant into a chosen category of the event .	Participants	{categoryid}	201 Created-enrolment created 
400 Bad Request -invalid category 
401 Unauthorized 
404 Not Found-event/category does not exist 
409 Conflict-already enrolled 

GET	/api/usrs/enrolments	Lists all events the logged in Partcipants is currently enrolled in .	Participants	None	200 OK-list of enrolments returned 
401 Unauthorized
DELTE	/api/enrolements/{id}	Cancels the logged in Participants owns enrolment	Participants	None	200 OK-list of participants returned 
403 Forbidden-not the owner
404 Not found
GET	api/events/{id}/enrolments	Lists all participants enrolled in an Organizer’s event	Organizer	None	200 OK-list of participants returned 
403 Forbidden-not the event owner
404 Not Found 

HTTP Method	Route	Desrcption	Role Required	Request Body	Expected Response
Results
POST	/api/events/{id}/results	Captures finish times and position for participants after an event has taken place 	Organizer	{ results :[{ participantid, categoryid,finishTime position }} }	201 Created -results recorded 
400 Bad Request -invalid data 
403 Forbidden-not the event owner 
404 Not Found 
GET	/ api/events/{id}/results	Returns the public results/leaderboard for a completed event 	None	None	200 OK-list of results returned 
404 Not Found-event does not exist 
PUT	/api/results/{id}	Corrects a single participant captured result.	Organizer	{ fisnishTime , position }	200 OK-results updated 
400 Bad Request 
403 Forbidden
404 Not Found 


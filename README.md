# boat-app
Boat App MVC dot net with ReactNet


To run this application locally:
Open the docker file and build it with docker running in the background.
I modified the run configurations as below which works for the ports on my machine and with the setup specified.

![Screenshot 2024-01-29 at 20 08 45](https://github.com/rdcranfield/boat-app/assets/36520843/3e419b25-dac1-408b-963f-0720eec54feb)

Go to your browser of choice and enter http://localhost:5031/
- You can double click the text of the boat name, length and width to modify the values, when you click off or press enter then the value will be updated.


There is also another project which contains some Unit Test Cases. Simply run the unit cases by opening the files and running or debugging them.



Improvements I would like to add in future.
- Better UI design with CSS and more interactive elements plus a website structure.
- Middleware for the networking errors to ensure consistency in error responses and centralisation of unhandled throws.
i.e. a common network project with client and server errors and app.UseMiddleware<NetExceptionMiddleware> in the startup file.
- Integration Testing and testing on a production/live db.

- More advanced demonstration of boat variety such as types, sizes and cargo for example. 

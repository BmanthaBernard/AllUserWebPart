All User Webs Webpart
=====================

This web part will search thru an entire web application and build a list of all the Webs that the user has permissions to access.  

This tool was created to enable easy navigation for users who need to collaborate in webs that are spread across numerous site collections. 

Key Features:
*   The user is presented with a sorted list of webs that they have permission to.
*   The webpart allows for regular expressions matching in the path to allow for trimming of results.

Performance Considerations:
*   This builds a list by scanning all webs in a web application.  Don’t put this on your landing page as it is expensive to run.  We recommend creating a separate "My Webs" page and providing the user a link to the page.
# nightingale-server

# HTTP-Routes

## /auth
* /
    * Get - Check authentification status
* /register
    * Post - Create new user account
        * {
            * string Email
            * string Password
        * }
* /login
    * Post - Log in to an existing account
        * {
            * string Email
            * string Password
        * }
* /logout
    * Post - Log out from current account (delete cookie)
    
 ## /user
* /username
   * Post - Change username of current user
        * {
            * string username
        * }
* /publicusername
    * Post - Change public username of current user
        * {
            * string NewPublicUserName
        * }
* /password
    * Post - Change password of current user
        * {
            * string oldPwd
            * string newPwd
        * }
    
# UserHub methods
* Send - send text message using ReceiverId or ContactId
    * {
        * int? ContactId
        * string? ReceiverId
        * string Text
        * string Time
    * }
    * Invokes ReceiveMessage method of client
* GetLastMessages - get last 20 messages of existing contact using Contactid
    * int contactId
    * Invokes GetMessages method of client
* GetMessagesBefore - get 20 previous messages from messageId in contactId
    * int contactId
    * int messageId
    * Invokes GetMessages method of client
* GetContacts - get all current user contacts
    * Invokes GetContacts method of client
* FindByPublicNickName - get all users with similar PublicNickName
    * string SearchString
    * Invokes RetrieveSearchResults method of client
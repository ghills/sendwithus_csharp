sendwithus_sharp
================

sendwithus C# Client

## status
[![Build status](https://ci.appveyor.com/api/projects/status/2f6ljyg5a6y1umba/branch/master?svg=true)](https://ci.appveyor.com/project/bvanvugt/sendwithus-csharp/branch/master)

## requirements
    none
    
## API Coverage
With one exception, this client covers all of the sendwithus API calls documented at: https://www.sendwithus.com/docs/api#overview

The exception is the Multi-Langauge API calls; those API calls are not supported in this client.  If you require support for the Multi-Language API, please contact sendwithus: https://www.sendwithus.com/contact

Install via NuGet Package Manager
---------------------------------

If you are using Visual Studio 2015:
* In the Solution Explorer window, right click on your solution and select "Manage NuGet Packages for Solution..."
* Search for "SendwithusClient"
 * Be sure to use the one that's by sendwithus.  There's another one called SendWithUs.Client by Mimeo, but that is not supported by sendwithus
* Select the "SendwithusClient" package and choose "Install" for your solution/project.


## Getting started

```csharp
using sendwithus;

// Set the API Key
SendwithusClient.ApiKey = <your_api_key> // Test or Production

// Configure API settings (optional)
SendwithusClient.SetTimeoutInMilliseconds(your_preferred_timeout); // Default is 30000 (30s).  This is the timeout for an individual API call attempt
SendwithusClient.RetryCount = your_preferred_retryCount; // Default is 3.  This is the number of times that each API call will be retried if it fails due to a potentially transient event
SendwithusClient.RetryIntervalMilliseconds = your_preferred_retryInterval; // Default is 100ms.  This is the amount of time to wait between retry attempts.

```

### Notes on Retries
The API will perform a retry in the following cases:

| Retry Causes | Exception Thrown |
| ------------ | ---------------- |
| API Call Timeout | TaskCanceledException |
| Received HTTP Status 502:  BadGateway | SendwithusException |
| Received HTTP Status 503: ServiceUnavailable | SendwithusException |
| Received HTTP Status 505: GatewayTimeout | SendwithusException |

A SendwithusException is a regular Exception with a StatusCode property added

All exceptions will be part of an AggregateException, regardless of whether a retry was attempted or not.

Some examples: 
* An API call that fails with a status code 403: Forbidden (not a retriable status code) will throw an AggregateException with one InnerException of type SendwithusException
* An API call that repeatedly times out will throw an AggregateException with InnerExceptions of type TaskCanceledException
* An API call that times out once, then fails with a status code of 503: Service Unavailable, and then fails again with a status code 400: Bad Request will throw an AggregateException with InnerExceptions, in order, of: { TaskCanceledException, SendwithusException, SendwithusException}

# API Calls
## Templates
### Get a list of templates
#### GET /templates
```csharp
try
{
    var templates = await Template.GetTemplatesAsync();
}
catch (AggregateException exception)
{
    // Exception handling
}
```
### Get a specific template (all versions)
#### GET /templates/(:template_id)
```csharp
var templateId = "tem_SxZKpxJSHPbYDWRSQnAQUR";
try
{
    var response = await Template.GetTemplateAsync(templateId);
}
catch (AggregateException exception)
{
    // Exception handling
}
```
#### GET /templates/(:template_id)/versions
```csharp
var templateId = "tem_SxZKpxJSHPbYDWRSQnAQUR";
try
{
    var template = await Template.GetTemplateVersionsAsync(templateId);
}
catch (AggregateException exception)
{
    // Exception handling
}
```
### Get a list of template versions (with HTML/text)
#### GET /templates/(:template_id)/versions
```csharp
var templateId = "tem_SxZKpxJSHPbYDWRSQnAQUR";
var locale = "en-US";
try
{
    var template = await Template.GetTemplateAsync(templateId, locale);
}
catch (AggregateException exception)
{
    // Exception handling
}
```
#### GET /templates/(:template_id)/locales/(:locale)/versions
```csharp
var templateId = "tem_SxZKpxJSHPbYDWRSQnAQUR";
var locale = "en-US";
try
{
    var templateVersions = await Template.GetTemplateVersionsAsync(templateId, locale);
}
catch (AggregateException exception)
{
    // Exception handling
}
```
### Get a specific version (with HTML/text)
#### GET /templates/(:template_id)/versions/(:version_id)
```csharp
var templateId = "tem_SxZKpxJSHPbYDWRSQnAQUR";
var versionId = "ver_ET3j2snkKhqsjRjtK6bXJE";
try
{
    var templateVersion = await Template.GetTemplateVersionAsync(templateId, versionId);
}
catch (AggregateException exception)
{
    // Exception handling
}
```
#### GET /templates/(:template_id)/locales/(:locale)/versions/(:version_id)
```csharp
var templateId = "tem_SxZKpxJSHPbYDWRSQnAQUR";
var locale = "en-US";
var versionId = "ver_ET3j2snkKhqsjRjtK6bXJE";
try
{
    var templateVersion = await Template.GetTemplateVersionAsync(templateId, locale, versionId);
}
catch (AggregateException exception)
{
    // Exception handling
}
```
### Update a template version
#### PUT /templates/(:template_id)/versions/(:version_id)
```csharp
var templateId = "tem_SxZKpxJSHPbYDWRSQnAQUR";
var versionId = "ver_ET3j2snkKhqsjRjtK6bXJE";
var updatedTemplateVersion = new TemplateVersion();
var templateVersionName = "New Version";
var templateSubject = "edited!";
var updatedTemplateVersion = new TemplateVersion(templateVersionName, templateSubject);
updatedTemplateVersion.html = "<html><head></head><body><h1>UPDATE</h1></body></html>"; // optional
updatedTemplateVersion.text = "sometext"; // optional
try
{
    var templateVersion = await Template.UpdateTemplateVersionAsync(templateId, versionId, updatedTemplateVersion);
}
catch (AggregateException exception)
{
    // Exception handling
}
```
#### PUT /templates/(:template_id)/locales/(:locale)/versions/(:version_id)
```csharp
var templateId = "tem_SxZKpxJSHPbYDWRSQnAQUR";
var locale = "en-US"
var versionId = "ver_ET3j2snkKhqsjRjtK6bXJE";
var updatedTemplateVersion = new TemplateVersion();
var templateVersionName = "New Version";
var templateSubject = "edited!";
var updatedTemplateVersion = new TemplateVersion(templateVersionName, templateSubject);
updatedTemplateVersion.html = "<html><head></head><body><h1>UPDATE</h1></body></html>"; // optional
updatedTemplateVersion.text = "sometext"; // optional
try
{
    var templateVersion = await Template.UpdateTemplateVersionAsync(templateId, versionId, updatedTemplateVersion);
}
catch (AggregateException exception)
{
    // Exception handling
}
```
### Create a New Template
#### POST /templates
```csharp
var templateVersionName = "New Template Version";
var templateSubject = "New Version!";
var updatedTemplateVersion = new TemplateVersion(templateVersionName, templateSubject);
updatedTemplateVersion.html = "<html><head></head><body><h1>NEW TEMPLATE VERSION</h1></body></html>"; // optional
updatedTemplateVersion.text = "some text"; // optional
updatedTemplateVersion.locale = "en-US"; // optional
try
{
    var template = await Template.CreateTemplateAsync(updatedTemplateVersion);
}
catch (AggregateException exception)
{
    // Exception handling
}
```
### Add locale to existing template
#### POST /templates/(:template_id)/locales
```csharp
var templateId = "tem_SxZKpxJSHPbYDWRSQnAQUR";
var locale = "fr-FR";
var templateVersionName = "Published French Version";
var templateSubject = "Ce est un nouveau modèle!";
var updatedTemplateVersion = new TemplateVersion(templateVersionName, templateSubject);
updatedTemplateVersion.html = "<html><head></head><body><h1>Nouveau modèle!</h1></body></html>"; // optional
updatedTemplateVersion.text = "un texte"; // optional
try
{
    var template = await Template.AddLocaleToTemplate(templateId, locale, updatedTemplateVersion);
}
catch (AggregateException exception)
{
    // Exception handling
}
```
### Create a new template version
#### POST /templates/(:template_id)/versions
```csharp
var templateId = "tem_SxZKpxJSHPbYDWRSQnAQUR";
var templateVersionName = "New Template Version";
var templateSubject = "New Version!";
var updatedTemplateVersion = new TemplateVersion(templateVersionName, templateSubject);
updatedTemplateVersion.html = "<html><head></head><body><h1>NEW TEMPLATE VERSION</h1></body></html>"; // optional
updatedTemplateVersion.text = "some text"; // optional
try
{
    var templateVersion = await Template.CreateTemplateVersion(templateId, updatedTemplateVersion);
}
catch (AggregateException exception)
{
    // Exception handling
}
```
#### POST /templates/(:template_id)/locales/(:locale)/versions
```csharp
var templateId = "tem_SxZKpxJSHPbYDWRSQnAQUR";
var locale = "en-US";
var templateVersionName = "New Template Version";
var templateSubject = "New Version!";
var updatedTemplateVersion = new TemplateVersion(templateVersionName, templateSubject);
updatedTemplateVersion.html = "<html><head></head><body><h1>NEW TEMPLATE VERSION</h1></body></html>"; // optional
updatedTemplateVersion.text = "some text"; // optional
try
{
    var templateVersion = await Template.CreateTemplateVersion(templateId, locale, updatedTemplateVersion);
}
catch (AggregateException exception)
{
    // Exception handling
}
```
### Delete a specific template
#### DELETE /templates/(:template_id)
```csharp
var templateId = "tem_SxZKpxJSHPbYDWRSQnAQUR";
try
{
    var genericApiCallStatus = await Template.DeleteTemplate(templateId);
}
catch (AggregateException exception)
{
    // Exception handling
}
```
#### DELETE /templates/(:template_id)/locales/(:locale)
```csharp
var templateId = "tem_SxZKpxJSHPbYDWRSQnAQUR";
var locale = "en-US";
try
{
    var genericApiCallStatus = await Template.DeleteTemplate(templateId, locale);
}
catch (AggregateException exception)
{
    // Exception handling
}
```
## Sending Emails

### Send an email
_We validate all HTML content_
#### POST /send
Example with only the required parameters:
```csharp
var templateId = "tem_SxZKpxJSHPbYDWRSQnAQUR";

// Construct the template data
// The content of the template data is all optional and is based on the template being used
var templateData = new Dictionary<string, object>();
templateData.Add("first_name", "Chuck");
templateData.Add("last_name", "Norris");
templateData.Add("img", "http://placekitten.com/50/60");
var link = new Dictionary<string, string>();
link.Add("url", "https://www.sendwithus.com");
link.Add("text", "sendwithus!");
templateData.Add("link", link);

// Construct the recipient
var recipient = new EmailRecipient(DEFAULT_RECIPIENT_EMAIL_ADDRESS);

// Construct the email object
var email = new Email(templateId, templateData, recipient);

// Send the email
try
{
    var emailResponse = await email.Send();
}
catch (AggregateException exception)
{
    // Exception handling
}
```
Example with all parameters:
```csharp
var templateId = "tem_SxZKpxJSHPbYDWRSQnAQUR";

// Construct the template data
// The content of the template data is all optional and is based on the template being used
var templateData = new Dictionary<string, object>();
templateData.Add("first_name", "Chuck");
templateData.Add("last_name", "Norris");
templateData.Add("img", "http://placekitten.com/50/60");
var link = new Dictionary<string, string>();
link.Add("url", "https://www.sendwithus.com");
link.Add("text", "sendwithus!");
templateData.Add("link", link);

// Construct the recipient
var recipient = new EmailRecipient(DEFAULT_RECIPIENT_EMAIL_ADDRESS);

// Construct the email object
var email = new Email(templateId, templateData, recipient);
email.cc.Add(new EmailRecipient("cc_one@email.com", "CC One"));
email.cc.Add(new EmailRecipient("cc_two@email.com", "CC Two"));
email.bcc.Add(new EmailRecipient("bcc_one@email.com", "BCC One"));
email.bcc.Add(new EmailRecipient("bcc_two@email.com", "BCC Two"));
email.sender.address = "company@company.com";
email.sender.reply_to = "info@company.com";
email.sender.name = "Company";
email.tags.Add("tag1");
email.tags.Add("tag2");
email.tags.Add("tag3");
email.headers.Add("X-HEADER-ONE", "header-value");
email.inline.id = "cat.png";
email.inline.data = "{BASE_64_ENCODED_FILE_DATA}";
email.files.Add(new EmailFileData("doc.txt", "{BASE_64_ENCODED_FILE_DATA}"));
email.files.Add(new EmailFileData("stuff.zip", "{BASE_64_ENCODED_FILE_DATA}"));
email.version_name = "this version";
email.locale = "en-US";
email.esp_account = "esp_EsgkbqQdDg7F3ncbz9EHW7";

// Send the email
try
{
    var emailResponse = await email.Send();
}
catch (AggregateException exception)
{
    // Exception handling
}
```
## Logs
### Get a list of logs
#### GET /logs
Example with no filter parameters:
```csharp
try
{
    var logs = await Log.GetLogsAsync();
}
catch (AggregateException exception)
{
    // Exception handling
}
```
Example with all the filter parameters:
```csharp
// Build the query parameters (each is optional)
Dictionary<string, object> queryParameters = new Dictionary<string, object>();
queryParameters.Add("count", DEFAULT_COUNT);
queryParameters.Add("offset", DEFAULT_OFFSET);
queryParameters.Add("created_gt", LOG_CREATED_AFTER_TIME);
queryParameters.Add("created_gte", LOG_CREATED_AFTER_TIME);
queryParameters.Add("created_lt", LOG_CREATED_BEFORE_TIME);
queryParameters.Add("created_lte", LOG_CREATED_BEFORE_TIME);

// Make the API call
try
{
    var logs = await Log.GetLogsAsync(queryParameters);
}
catch (AggregateException exception)
{
    // Exception handling
}
```
### Get a specific log + metadata
#### GET /logs/(:log_id)
```csharp
var logId = "log_88be2c0f8b5c6d3933dd578b6a0f13e5";
try
{
    var log = await Log.GetLogAsync(logId);
}
catch (AggregateException exception)
{
    // Exception handling
}
```
### Retrieve events for a specific log_id
#### GET /logs/(:log_id)/events
```csharp
var logId = "log_88be2c0f8b5c6d3933dd578b6a0f13e5";
try
{
    var logEvents = await Log.GetLogEventsAsync(logId);
}
catch (AggregateException exception)
{
    // Exception handling
}
```
### Resend an existing Log
#### POST /resend
```csharp
var logId = "log_88be2c0f8b5c6d3933dd578b6a0f13e5";
try
{
    var logResendResponse = await Log.ResendLogAsync(logId);
}
catch (AggregateException exception)
{
    // Exception handling
}
```
## Snippets
### Get all snippets
#### GET /snippets
```csharp
try
{
    var snippets = await Snippet.GetSnippetsAsync();
}
catch (AggregateException exception)
{
    // Exception handling
}
```
### Get specific snippet
#### GET /snippets/(:id)
```csharp
var snippetId = "snp_bn8c87iXuFWdtYLGJrBAWW";
try
{
    var snippets = await Snippet.GetSnippetAsync(snippetId);
}
catch (AggregateException exception)
{
    // Exception handling
}
```
### Creating a new snippet
#### POST /snippets
```csharp
var snippetName = "My First Snippet";
var snippetBody = "<h1>Welcome!</h1>";
try
{
    var snippetResponse = await Snippet.CreateSnippetAsync(snippetName, snippetBody);
}
catch (AggregateException exception)
{
    // Exception handling
}
```
### Update an existing snippet
#### PUT /snippets/(:id)
```csharp
var snippetId = "snp_bn8c87iXuFWdtYLGJrBAWW";
var snippetName = "Updated Snippet";
var snippetBody = "<h1>Welcome Again!</h1>";
try
{
    var response = await Snippet.UpdateSnippetAsync(snippetId, snippetName, snippetBody);
}
catch (AggregateException exception)
{
    // Exception handling
}
```
### Delete an existing snippet
#### DELETE /snippets/(:id)
```csharp
var snippetId = "snp_bn8c87iXuFWdtYLGJrBAWW";
try
{
    var genericApiCallStatus = await Snippet.DeleteSnippetAsync(snippetId);
}
catch (AggregateException exception)
{
    // Exception handling
}
```



### Get emails

```php
$response = $api->emails();
```

## Get specific template
```php
$response = $api->get_template($template_id,     //string id of template
                               $version_id       //optional string version id of template
);
```


## Create emails
### Create new email
_We validate all HTML content_
```php
$response = $api->create_email('Email Name',               // string email name
    'Email Subject',                                       // string subject line of email
    '<html><head></head><body>Valid HTML<body></html>',    // string of HTML code for email
    'Optional text content')                               // optional string of text for email
```

### Create new email template version
_We validate all HTML content_
```php
$response = $api->create_new_template_version(
    'Email Name',                                          // string email version name
    'Email Subject',                                       // string subject of email
	'tem_JAksjdjwJXUVwnemljflksEJks',                      // string id of email used
    '<html><head></head><body>Valid HTML<body></html>',    // string block of HTML code used for email
    'Optional text content')                               // optional string of text used for email
```

### Update email version
_We validate all HTML content_
```php
$response = $api->update_template_version(
    'Email Name',                                          // string email version name
    'Email Subject',                                       // string subject of email
	'tem_JAkCjdjwJXUVwnemljflksEJks',                      // string id of email being updated
	'ver_iuweJskj4Jwkj2ndclk4jJDken',                      // string version of email being updated
    '<html><head></head><body>Valid HTML<body></html>',    // string block of HTML code used for email
    'Optional text content')                               // optional string of text used for email
```


## Send emails

*NOTE* - If a customer does not exist by the specified email (recipient address), the send call will create a customer.

```php
// Send function header
send(
    $email_id,      // string, id of email to send (template id)
    $recipient,     // associative array, ("address" => "ckent@dailyplanet.com", "name" => "Clark") to send to
    $args           // (optional) array, (array) additional parameters - (see below)
)

// Send function options
'template_data'  // array of variables to merge into the template.
'sender'         // array ("address", "name", "reply_to") of sender.
'cc'             // array of ("address", "name") for carbon copy.
'bcc'            // array of ("address", "name") for blind carbon copy.
'inline'         // string, path to file to include inline.
'files'          // array, paths to files to attach to the send.
'tags'           // array of strings to tag email send with.
'esp_account'    // string of ESP ID to manually select ESP
```

## Send Examples

### Send request with REQUIRED parameters only

```php
$response = $api->send('email_id',
    array('address' => 'us@sendwithus.com')
);
```

### Send request with REQUIRED and OPTIONAL parameters

```php
$response = $api->send('email_id',
    array(
        'name' => 'Matt',
        'address' => 'us@sendwithus.com'),
    array(
    	'template_data' => array('name' => 'Jimmy the snake'),
    	'sender' => array(
            'name' => 'Company',
            'address' => 'company@company.com',
            'reply_to' => 'info@company.com'
        ),
        'esp_account' => 'esp_EMpi5eo59cG4cCWd7AdW7J'
    )
);
```

### Send an email with multiple CC/BCC recipients

```php
$response = $api->send('email_id',
    array(
        'name' => 'Matt',
        'address' => 'us@sendwithus.com'
    ),
    array(
        'template_data' => array('name' => 'Jimmy the snake'),
        'sender' => array(
            'name' => 'Company',
            'address' => 'company@company.com',
            'reply_to' => 'info@company.com'
        ),
        'cc' => array(
            array(
                'name' => 'CC Name',
                'address' => 'CC@company.com'
            ),
            array(
                'name' => 'CC 2 Name',
                'address' => 'CC2@company.com'
            )
        ),
        'bcc' => array(
            array(
                'name' => 'BCC Name',
                'address' => 'BCC@company.com'
            )
        )
    )
);
```

### Send an email with a dynamic tag

```php
$response = $api->send('email_id',
    array(
        'name' => 'Matt',
        'address' => 'us@sendwithus.com'),
    array(
        'tags' => array('Production', 'Client1')
    )
);
```

### Send specific version of an email

```php
$response = $api->send('email_id',
    array(
        'name' => 'Matt',
        'address' => 'us@sendwithus.com'),
    array(
        'version_name' => 'My Version'
    )
);
```

### Send email with an inline image attachment

```php
$response = $api->send('email_id',
    array(
        'name' => 'Matt',
        'address' => 'us@sendwithus.com'),
    array(
        'inline' => 'filename.jpg'
    )
);
```

### Send email with attachments

```php
$response = $api->send('email_id',
    array(
        'name' => 'Matt',
        'address' => 'us@sendwithus.com'),
    array(
        'files' => array(
            'filename.txt',
            'filename.pdf'
        )
    )
);
```


## Render templates

```php
// Render function header
render(
    $email_id,      // string, id of email to send (template id)
    $args           // (optional) array, (array) additional parameters - (see below)
)

// Send function options
'template_data'  // Array of variables to merge into the template.
'version_id'     // Version ID obtained from /templates/(:template_id)/versions
'version_name'   // Version name that you want rendered (provide either a version_name or a version_id, not both)
'locale'         // Template locale to render
'strict'         // Render in strict mode (fails on missing template data)
```

### Example:

```php
$response = $api->send('email_id',
    array('address' => 'us@sendwithus.com'),
    array(
        'template_data' => array(
            'name' => 'Bobby Boucher'
        )
    )
);
```


## Segments

### Get segments
```php
get_segments()
```

Example

```php
$response = $api->get_segments();
```

Response

```php
Array
(
    [0] => stdClass Object
        (
            [created] => 1402378620
            [id] => seg_0biVV4Ncf1234
            [name] => TEST_SEGMENT
            [object] => segment
        )

)
```

### Send to a Segment
```php
send_segment(
    $email_id,          // id of template to send
    $segment_id,        // id of the segment to send to
    $data               // optional array of data to send
)
```

Example

```php
$response = $api->send_segment(tem_123jeDI23, 'seg_0biVV4Ncf1234');
```

## Get Email Logs
```php
logs(
    $count             //The number of logs to return. Max: 100, Default: 100
    $offset            //Offset the number of logs to return. Default: 0
    $created_gt        //Return logs created strictly after the given UTC timestamp
    $created_gte       //Return logs created on or after the given UTC timestamp
    $created_lt        //Return logs created strictly before the given UTC timestamp
    $created_lte       //Return logs created on or before the given UTC timestamp
)
```
## Get a Specific Email's Log
```php
get_log(
    $log_id          // id of log to retrieve
)
```

Example

```php
$response = api->get_log('log_d4R7hV4d0r')
```

Response

```php
(
    [email_id] => tem_1jeid84bg
    [recipient_name] =>
    [message] => Mandrill: Message has been successfully delivered to the receiving server.
    [id] => log_d4R7hV4d0r
    [object] => log
    [created] => 1409287597
    [email_name] => test
    [recipient_address] => person@example.com
    [status] => sent
    [email_version] => Original Version
)
```

## Drip Unsubscribe
```php
// Unsubscribe email address from active drips
drip_unsubscribe(
    $email_address,      // the email to unsubscribe from active drips
)
```

## Drip Unsubscribe Example

```php
$response = $api->drip_unsubscribe('us@sendwithus.com');
```

## Drips 2.0

### List Drip Campaigns
List all drip campaigns for the current profile

Example

```php
$response = $api->list_drip_campaigns();
```

Response

```php
Array
(
    [0] => stdClass Object
        (
            [drip_steps] => Array
                (
                    [0] => stdClass Object
                        (
                            [id] => dcs_1234abcd1234
                            [object] => drip_step
                            [delay_seconds] => 0
                            [email_id] => tem_1234abcd1234
                        )

                )

            [name] => Drip Campaign
            [enabled] => 1
            [id] => dc_1234abcd1234
            [trigger_email_id] => tem_1234abcd1234
            [object] => drip_campaign
        )
)
```

### Start on Drip Campaign
Starts a customer on the first step of a specified drip campaign

```php
start_on_drip_campaign(
    $recipient_address, // string, email address being added to drip campaign
    $drip_campaign_id,  // string, drip campaign being added to
	$data               // array, (optional) email data being added to drip campaign
	$args               // array, (optional) additional options being sent with email (tags, cc's, etc)
);

// Args options
'sender'      // array ("address", "name", "reply_to") of sender.
'cc'          // array of ("address", "name") for carbon copy.
'bcc'         // array of ("address", "name") for blind carbon copy.
'tags'        // array of strings to tag email send with.
'esp_account' // string of ESP ID to manually select ESP
```

Example

```php
$template_data = array(
    'name' => 'Jean-Luc'
	'rank' => 'Captain'
);

$args = array(
    'tags' => array('all', 'the', 'tags'),
    'cc' => array('address' => 'them@sendwithus.com')
);
$response = $api->start_on_drip_campaign('us@sendwithus.com', 'dc_1234abcd1234', $template_data, $args);
```

Response

```php
stdClass Object
(
    [success] => 1
    [drip_campaign] => stdClass Object
        (
            [id] => dc_1234abcd1234
            [name] => Drip Campaign
        )

    [message] => Recipient successfully added to drip campaign.
    [status] => OK
    [recipient_address] => us@sendwithus.com
)
```

### Remove from Drip Campaign
Deactivates all pending emails for a customer on a specified drip campaign
```php
$response = $api->remove_from_drip_campaign(
    $recipient_address, // string, email address being removed from drip campaign
    $drip_campaign_id   // string, drip campaign being removed from
);
```

Example

```php
$response = $api->remove_from_drip_campaign('us@sendwithus.com', 'dc_1234abcd1234');
```

Response

```php
stdClass Object
(
    [success] => 1
    [drip_campaign] => stdClass Object
        (
            [id] => dc_1234abcd1234
            [name] => Drip Campaign
        )

    [message] => Recipient successfully removed from drip campaign.
    [status] => OK
    [recipient_address] => us@sendwithus.com
)
```

### List Drip Campaign Details
Show all the steps and other information in a specified campaign
```php
$response = $api->drip_campaign_details(
    $drip_campaign_id   // string, drip campaign to list details from
);
```

Example

```php
$response = $api->drip_campaign_details('dc_1234abcd1234');
```

Response

```php
stdClass Object
(
    [drip_steps] => Array
        (
            [0] => stdClass Object
                (
                    [id] => dcs_1234abcd1234
                    [object] => drip_step
                    [delay_seconds] => 0
                    [email_id] => tem_1234abcd1234
                )

        )

    [name] => Drip Campaign
    [enabled] => 1
    [id] => dc_1234abcd1234
    [trigger_email_id] => tem_1234abcd1234
    [object] => drip_campaign
)

```

## ESP Accounts API

### Get ESP accounts
```php
// possible esp_type values are -
// sendgrid
// mailgun
// mandrill
// postmark
// ses
// mailjet
// dyn
// sparkpost
// smtp

get_esp_accounts(
    $esp_type,      // string, optional, filter response to only return ESP accounts of a certain type
    $count,         // integer, optional, the number of logs to return. Max: 100, Default: 100
    $offset         // integer, optional, Offset the number of logs to return. Default: 0
)
```

Example

```php
$response = $api->get_esp_accounts('mailgun');
```

### Create ESP account
```php
// possible esp_type values are -
// sendgrid
// mailgun
// mandrill
// postmark
// ses
// mailjet
// dyn
// sparkpost
// smtp

create_esp_account(
    $name,          // string, the name of your account
    $esp_type,      // string, Must be one of the options listed above
    $credentials    // array, account credentials of the new esp account. See docs for formats
)
```

Example

```php
// see the docs for formats
// https://www.sendwithus.com/docs/api#email-service-providers

$credentials = array(
    'username' => 'mysendgridusername',
    'password' => 'mysendgridpassword'
);

$response = $api->create_esp_account(
    'My SendGrid Account',
    'sendgrid',
    $credentials
);
```

### Set default ESP account
```php
set_default_esp_account(
    '$esp_id'       // string, the ID of the ESP account
)
```

Example

```php
$response = $api->set_default_esp_account('esp_asdf12rastrast');
```

## Customers API

### Create Customer
```php
create_customer(
    $email,             // string, email of customer
    $data,              // array, optional, data for customer
    $args               // array, optional, optional parameters:

    // The additional optional parameters are as follows:
    //      'locale' - Default is null. String to specify a locale for this customer.
    //      'groups' - Default is null. Array of group IDs
)
```

Example

```php
$response = $api->create_customer('us@sendwithus.com',
    array('name' => 'Sendwithus')
);
```

### Update Customer
```php
update_customer(
    $email,             // string, email of customer
    $data,              // array, optional, data for customer
)
```

Example

```php
$response = $api->update_customer('us@sendwithus.com',
    array('name' => 'Sendwithus.com')
);
```

### Delete Customer
```php
delete_customer(
    $email,             // string, email of customer
)
```

Example

```php
$response = $api->delete_customer('us@sendwithus.com');
```

### Conversion on Customer
Adds a conversion event to recent templates for recipient

```php
customer_conversion(
    $email,             // string, email of customer
    $revenue,           // integer, optional, amount in cents
);
```

Example (required parameters only)

```php
$response = $api->customer_conversion('curtis@sendwithus.com');

print_r($response);

/*
stdClass Object
(
    [status] => OK
    [success] => 1
)
*/
```

Example (all parameters)

```php
// $19.99 in revenue
$response = $api->customer_conversion('curtis@sendwithus.com', 1999);

print_r($response);

/*
stdClass Object
(
    [status] => OK
    [success] => 1
)
*/
```

### Add Customer to Group
Adds a customer to a specified group

```php
add_customer_to_group('curtis@sendwithus.com', 'grp_jk2jejidi3');
```

### Remove Customer From Group
Removes a customer from a specified group

```php
remove_customer_from_group('curtis@sendwithus.com', 'grp_jk2jejidi3');
```

## Groups API

### List Groups
List all groups

Example

```php
$response = $api->list_groups();

print_r($response->groups[0]);

/*
(
    [id] => grp_nEozfaiWwBHW4Fsmc
    [description] => An example group
    [name] => Group name
)
*/

```

### Create a Group
```php
create_group(
    $name,             // string, name of group
    $description,      // string, optional, description for group
)
```

Example

```php
$response = $api->create_group('Group name',
    'An example group'
);

print_r($response);

/*
stdClass Object
(
    [success] => 1
    [group] => stdClass Object
        (
            [name] => Group name
            [description] => An example group
            [id] => grp_ooEDQKetS2Yqs7FSGAdReB
        )

    [status] => OK
)
*/
```

### Update a Group
```php
update_group(
    $name,             // string, name of group
    $group_id          // string id of group
    $description,      // string, optional, description for group
)
```

Example

```php
$response = $api->update_group('Updated Group name',
    'An example group updated description'
);

print_r($response);

/*
stdClass Object
(
    [success] => 1
    [group] => stdClass Object
        (
            [name] => Updated Group name
            [description] => An example group updated description
            [id] => grp_ooEDQKetS2Yqs7FSGAdReB
        )

    [status] => OK
)
*/
```

### Delete Group
```php
delete_group(
    $group_id,             // string, group_id of group
)
```

Example

```php
$response = $api->delete_group('grp_ooEDQKetS2Yqs7FSGAdReB');
```


## Expected response

### Success
```php
print $response->success; // true

print $response->status; // "OK"

print $response->receipt_id; // ### numeric receipt_id you can use to query email status later
```

### Error cases
```php
print $response->success; // false

print $response->status; // "error"

print $response->exception; // Exception Object

print $response->code;
// 400 (malformed request)
// 403 (bad api key)
```

### List Customer Logs
List all customer logs

Example

```php
$response = api->get_customer_logs("email@email.com");

print_r($response);

/*
(
    [success] => 1
    [logs] => Array
        (
        [email_name] => Name of email
        [message] => Message body
        [recipient_name] => Recipient name
        [email_version] => Name of email version
        [object] => log
        [email_id] => ID of email
        [created] => Time stamp
        [recipient_address] => Email address of recipient
        [status] => Status of email
        [id] => ID of log
        )
    [status] => OK
)
*/

```

## Batch API
Batch requests together to be run all at once.

### Usage
Create a batch_api object by calling `start_batch()`.

Do any request you would do normally with the API but on the batch_api object.

Execute all commands at once by calling `execute()` on the object.

### Example
```php
$batch_api = api->start_batch();
for($i = 0; $i < 10; $i++) {
    $result = $batch_api->create_customer('us@sendwithus.com',
        array('name' => 'Sendwithus'));
    // $result->success == true && $result->status == 'Batched'
}
$result = $batch_api->execute();

// $result will be an array of responses for each command executed.

```

### Canceling Batch Request
Sometimes it is necessary to cancel all the api requests that have been batched, but not yet sent.
To do that, use `cancel()`:

### Example
```php
$batch_api = api->start_batch();
for($i = 0; $i < 10; $i++) {
    $batch_api->create_customer('us@sendwithus.com',
        array('name' => 'Sendwithus'));
}
$result = $batch_api->cancel();
// $result->success == true && $result->status == 'Canceled'
```

Once you have canceled a batch, you can continue to use the batch to make more requests.

## Tests

### Running Unit Tests

Make sure to have phpunit installed (http://phpunit.de/) and run the following from the root directory

```php
phpunit test
```

# Troubleshooting

## General Troubleshooting

-   Enable debug mode
-   Make sure you're using the latest PHP client
-   Make sure `data/ca-certificate.pem` is included. This file is *required*
-   Capture the response data and check your logs &mdash; often this will have the exact error

## Enable Debug Mode

Debug mode prints out the underlying cURL information as well as the data payload that gets sent to sendwithus. You will most likely find this information in your logs. To enable it, simply put `"DEBUG" => true` in the optional parameters when instantiating the API object. Use the debug mode to compare the data payload getting sent to [sendwithus' API docs](https://www.sendwithus.com/docs/api "Official Sendwithus API Docs").

```php
$API_KEY = 'THIS_IS_AN_EXAMPLE_API_KEY';
$options = array(
    'DEBUG' => true
);

$api = new API($API_KEY, $options);
```

## Response Ranges

Sendwithus' API typically sends responses back in these ranges:

-   2xx – Successful Request
-   4xx – Failed Request (Client error)
-   5xx – Failed Request (Server error)

If you're receiving an error in the 400 response range follow these steps:

-   Double check the data and ID's getting passed to sendwithus
-   Ensure your API key is correct
-   Make sure there's no extraneous spaces in the id's getting passed

*Note*: Enable Debug mode to check the response code.

## Gmail Delivery Issues

Sendwithus recommends using one of our [supported ESPs](https://support.sendwithus.com/esp_accounts/esp_compatability/ "ESP Compatibility Chart"). For some hints on getting Gmail working check [here](https://support.sendwithus.com/esp_accounts/can_i_use_gmail/ "Can I use Gmail?").

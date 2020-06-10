TwitchBotAWS


Here's the general overview... 

aws.twitchbot.container - 

contains necessary logic/code for a docker image that logs into twitch, scans for applicable messages (test code in there for now), and sends them to a queue on AWS SQS.


aws.twitchbot.aws - 

contains all aws cloud logic (queues, lambda, dynamodb, etc)

We load the docker container into an ECS cluster, so it can scale up and down based on the backlog of messages in the queue to process. We then tie a lambda to each queue (request, response) - to both process requests (requests are generic - they can be anything), and to send responses back to the associated user/channel. Each component can scale horizontally as necessary (to a configurable min/max) 


As this is basically a message queue, the messages can be whatever we want, and can be integrated into any of AWS' services or any other service (discord, twitch, feeds, etc). 

Current code state: Still working on infrastructure. Container is deployed to ECR and piped into CI/CD. AWS stack can be deployed, but might still have configuration errors.

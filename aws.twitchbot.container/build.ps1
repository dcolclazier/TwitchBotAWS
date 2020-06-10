dotnet publish -c Debug --self-contained -r linux-x64 -o "container"
docker build -t aws.twitchbot.container .

docker run aws.twitchbot.container:latest


#log in to ECR and push docker container so it can be accessed in CloudFormation template and by AWS elsewhere
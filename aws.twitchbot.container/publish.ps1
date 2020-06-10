dotnet publish -c Debug --self-contained -r linux-x64 -o "container"
docker build -t aws.twitchbot.container .

aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin 246716306459.dkr.ecr.us-east-1.amazonaws.com
docker tag aws.twitchbot.container:latest 246716306459.dkr.ecr.us-east-1.amazonaws.com/aws.twitch.container:latest
docker push 246716306459.dkr.ecr.us-east-1.amazonaws.com/aws.twitch.container:latest


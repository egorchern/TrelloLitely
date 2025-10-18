cd backend\EzyClassroomz.Api
docker build  --build-arg custom_environment=local -t ezyclassroomz/api  .
docker run -it -p 3444:8080 --name ezyclassroomz-api ezyclassroomz/api
docker run -p 5432:5432 --name dev-postgres -e POSTGRES_USER=root -e POSTGRES_PASSWORD=password -d postgres
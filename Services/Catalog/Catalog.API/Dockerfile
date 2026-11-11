# مرحلة التشغيل الأساسية (التي قمت بتوليدها بالفعل)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# مرحلة البناء
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# نسخ ملفات الـ .csproj لكل مشروع بناءً على المسار الحقيقي في الـ Solution
COPY ["Services/Catalog/Catalog.API/Catalog.API.csproj", "Services/Catalog/Catalog.API/"]
COPY ["Services/Catalog/Catalog.Application/Catalog.Application.csproj", "Services/Catalog/Catalog.Application/"]
COPY ["Services/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj", "Services/Catalog/Catalog.Infrastructure/"]
COPY ["Services/Catalog/Catalog.Core/Catalog.Core.csproj", "Services/Catalog/Catalog.Core/"]

RUN dotnet restore "Services/Catalog/Catalog.API/Catalog.API.csproj"

# نسخ باقي ملفات الكود وبناء المشروع
COPY . .
WORKDIR "/src/Services/Catalog/Catalog.API"
RUN dotnet build "Catalog.API.csproj" -c Release -o /app/build

# مرحلة النشر (Publish)
FROM build AS publish
RUN dotnet publish "Catalog.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# المرحلة النهائية لربط كل شيء معاً للتشغيل
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]
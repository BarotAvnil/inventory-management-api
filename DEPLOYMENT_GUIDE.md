# 🚀 Deploying Inventory Management API to Render

This guide will help you deploy your .NET Inventory Management API to Render for production use.

## 📋 Prerequisites

1. **GitHub Account** - Your code needs to be in a GitHub repository
2. **Render Account** - Sign up at [render.com](https://render.com)
3. **Git** installed on your machine

## 📁 Project Structure

Make sure your project has these files:
```
InventoryApi/
├── Dockerfile
├── render.yaml
├── .dockerignore
├── Program.cs (updated with CORS)
├── InventoryApi.csproj
└── ... (other project files)
```

## 🔄 Step 1: Push Code to GitHub

1. **Initialize Git repository** (if not already done):
   ```bash
   cd C:\Users\avnil\AndroidStudioProjects\inventory_management_system\InventoryApi_NoAudit\InventoryApi
   git init
   git add .
   git commit -m "Initial commit - Inventory Management API"
   ```

2. **Create GitHub repository**:
   - Go to [github.com](https://github.com) and create a new repository
   - Name it `inventory-management-api` or similar
   - **Don't initialize with README** (since you already have files)

3. **Connect and push**:
   ```bash
   git remote add origin https://github.com/YOUR_USERNAME/inventory-management-api.git
   git branch -M main
   git push -u origin main
   ```

## 🌐 Step 2: Deploy to Render

### Option A: Using Render Dashboard

1. **Sign in to Render**:
   - Go to [render.com](https://render.com)
   - Sign up/Sign in (you can use GitHub OAuth)

2. **Connect GitHub**:
   - Click "New +" → "Web Service"
   - Connect your GitHub account
   - Select your `inventory-management-api` repository

3. **Configure Service**:
   - **Name**: `inventory-api` (or your preferred name)
   - **Environment**: `Docker`
   - **Region**: Choose closest to your users (e.g., Oregon, Frankfurt)
   - **Plan**: Start with "Free" (can upgrade later)
   - **Dockerfile Path**: `./Dockerfile`

4. **Environment Variables** (set these in Render dashboard):
   ```
   ASPNETCORE_ENVIRONMENT=Production
   ASPNETCORE_URLS=http://0.0.0.0:8080
   ASPNETCORE_HTTP_PORTS=8080
   ```

5. **Deploy**:
   - Click "Create Web Service"
   - Render will automatically build and deploy your API
   - First deployment takes ~5-10 minutes

### Option B: Using render.yaml (Automatic)

If you have the `render.yaml` file in your repository root:

1. Go to Render Dashboard
2. Click "New +" → "Blueprint"
3. Connect your GitHub repository
4. Render will automatically read the `render.yaml` configuration
5. Review settings and click "Apply"

## 🔍 Step 3: Verify Deployment

1. **Check Health**:
   - Your API will be available at: `https://YOUR_SERVICE_NAME.onrender.com`
   - Test health endpoint: `https://YOUR_SERVICE_NAME.onrender.com/api/dashboard/health`

2. **Test API Endpoints**:
   ```bash
   # Replace YOUR_SERVICE_NAME with your actual service name
   curl https://YOUR_SERVICE_NAME.onrender.com/api/products
   curl https://YOUR_SERVICE_NAME.onrender.com/api/categories
   ```

3. **View Swagger Documentation**:
   - Visit: `https://YOUR_SERVICE_NAME.onrender.com/swagger`

## 📱 Step 4: Update Your App

Update your mobile app's base URL to point to your deployed API:

```dart
// For Flutter/Dart
const String API_BASE_URL = 'https://YOUR_SERVICE_NAME.onrender.com';

// For Android (Java/Kotlin)
public static final String API_BASE_URL = "https://YOUR_SERVICE_NAME.onrender.com";
```

## 🔧 Configuration Details

### Database
- Uses SQLite with persistent disk storage
- Database file stored in `/app/Data/ims.db`
- Automatically seeded with initial data on first run

### CORS
- Configured to allow all origins for development
- For production, consider restricting to your app's domains

### Health Check
- Endpoint: `/api/dashboard/health`
- Render uses this to monitor service health

## 🚨 Important Notes

### Free Plan Limitations
- **Sleep after 15 minutes** of inactivity
- **750 hours/month** runtime limit
- **Cold starts** - first request after sleep takes ~30 seconds
- **Limited bandwidth and storage**

### Production Considerations
- **Upgrade to paid plan** for production use
- **Add authentication** if not using anonymous access
- **Use PostgreSQL** instead of SQLite for better performance
- **Set up monitoring** and alerting
- **Configure custom domain** if needed

## 🔄 Updating Your API

When you make changes:

1. **Push to GitHub**:
   ```bash
   git add .
   git commit -m "Your commit message"
   git push
   ```

2. **Automatic Deployment**:
   - Render automatically detects changes
   - Rebuilds and redeploys your service
   - Takes ~3-5 minutes for updates

## 🐛 Troubleshooting

### Build Fails
- Check Render build logs
- Ensure all NuGet packages are compatible with .NET 9
- Verify Dockerfile syntax

### Service Won't Start
- Check application logs in Render dashboard
- Verify port configuration (should be 8080)
- Ensure database directory is writable

### Database Issues
- Check if disk is properly mounted at `/app/Data`
- Verify SQLite file permissions
- Monitor disk usage

### CORS Issues
- Verify CORS is enabled in Program.cs
- Check browser console for CORS errors
- Test API endpoints with curl/Postman

## 📞 Support

If you encounter issues:
1. Check Render documentation
2. Review application logs
3. Test locally with Docker first
4. Contact Render support for platform issues

## 🎉 Success!

Your Inventory Management API should now be live and accessible from anywhere! 

**Your API URL**: `https://YOUR_SERVICE_NAME.onrender.com`
**Swagger Docs**: `https://YOUR_SERVICE_NAME.onrender.com/swagger`

Remember to update your mobile app configuration to use the new production URL.

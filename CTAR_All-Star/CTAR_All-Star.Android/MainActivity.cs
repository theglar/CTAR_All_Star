using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.IO;
using CTAR_All_Star;
using Plugin.Permissions;
using Android.Support.V4.Content;
using Android;
using Android.Support.V4.App;
using Android.Util;
using Android.Support.Design.Widget;
using System.Threading.Tasks;

namespace CTAR_All_Star.Droid
{
    [Activity(Label = "CTAR All-Star", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override async void OnCreate(Bundle bundle)
        {
            await TryToGetPermissions();

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            string fileName = "measurements_db.sqlite";
            string fileLocation = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string full_path = Path.Combine(fileLocation, fileName);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(full_path));
        }

        // Permissions code below is from https://www.youtube.com/watch?v=Uzpy3qdYXmE

        async Task TryToGetPermissions()
        {
            if((int)Build.VERSION.SdkInt >= 23)
            {
                await GetPermissionsAsync();
                return;
            }
        }
        const int RequestLocationId = 0;

        readonly string[] PermissionsGroupLocation =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation,
        };

        async Task GetPermissionsAsync()
        {
            const string permission = Manifest.Permission.AccessFineLocation;

            if (CheckSelfPermission(permission) == (int)Android.Content.PM.Permission.Granted)
            {
                Toast.MakeText(this, "Permission Granted", ToastLength.Short).Show();
                return;
            }

            if(ShouldShowRequestPermissionRationale(permission))
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Permissions Needed");
                alert.SetMessage("CTAR All-Star needs permission to continue");
                alert.SetPositiveButton("Request Permissions", (senderAlert, args) =>
                {
                    RequestPermissions(PermissionsGroupLocation, RequestLocationId);
                });
                alert.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Cancelled", ToastLength.Short);
                });

                Dialog dialog = alert.Create();
                dialog.Show();

                return;
            }
            RequestPermissions(PermissionsGroupLocation, RequestLocationId);
        }
                

        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestLocationId:
                    if(grantResults[0] == (int)Android.Content.PM.Permission.Granted)
                    {
                        Toast.MakeText(this, "Permission Granted", ToastLength.Short).Show();
                    }
                    else
                    {
                        Toast.MakeText(this, "Permission Denied", ToastLength.Short).Show();
                    }
                    break;
            }            
        }
    }
}


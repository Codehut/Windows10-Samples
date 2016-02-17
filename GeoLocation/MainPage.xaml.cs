using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Popups;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GeoLocation
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void getLocationBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Request permission to get location
                //Pop up will occur if your app doesnot have permission to access location service of device
                var accessStatus = await Geolocator.RequestAccessAsync();

                switch (accessStatus)
                {
                    case GeolocationAccessStatus.Allowed:
                        //Continue Getting location
                        Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 50, DesiredAccuracy = PositionAccuracy.High }; //Set the desired accuracy that you want in meters
                        Geoposition pos = await geolocator.GetGeopositionAsync();

                        LoadPositionData(pos);
                        break;

                    case GeolocationAccessStatus.Denied:
                        //Unable to get data
                        LoadPositionData(null);
                        break;

                    case GeolocationAccessStatus.Unspecified:
                        //Unable to get data
                        LoadPositionData(null);
                        break;
                }
            }
            catch (Exception ex)
            {
                await new MessageDialog("Something went wrong. Could not get finish current operation.").ShowAsync();
            }
        }
        private async void getAddressBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Request permission to get location
                //Pop up will occur if your app doesnot have permission to access location service of device
                var accessStatus = await Geolocator.RequestAccessAsync();

                switch (accessStatus)
                {
                    case GeolocationAccessStatus.Allowed:
                        // The location to reverse geocode.
                        //For now i am getting my current location
                        Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 50, DesiredAccuracy = PositionAccuracy.High }; //Set the desired accuracy that you want in meters
                        Geoposition pos = await geolocator.GetGeopositionAsync();


                        BasicGeoposition location = new BasicGeoposition();
                        location.Latitude = pos.Coordinate.Point.Position.Latitude;
                        location.Longitude = pos.Coordinate.Point.Position.Longitude;

                        Geopoint pointToReverseGeocode = new Geopoint(location);

                        // Reverse geocode the specified geographic location.
                        MapLocationFinderResult result =
                              await MapLocationFinder.FindLocationsAtAsync(pointToReverseGeocode);

                        // If the query returns results, display the name of the town
                        // contained in the address of the first result.
                        if (result.Status == MapLocationFinderStatus.Success)
                        {
                            continent.Text = result.Locations[0].Address.Continent;
                            countryName.Text = result.Locations[0].Address.Country;
                            countryCode.Text = result.Locations[0].Address.CountryCode;
                            region.Text = result.Locations[0].Address.Region;
                            town.Text = result.Locations[0].Address.Town;
                            street.Text = result.Locations[0].Address.Street;
                        }
                        else
                        {
                            await new MessageDialog("Something went wrong. Could not get finish current operation. Error: " + result.Status).ShowAsync();
                        }
                        break;

                    case GeolocationAccessStatus.Denied:
                        //Unable to get data
                        LoadPositionData(null);
                        break;

                    case GeolocationAccessStatus.Unspecified:
                        //Unable to get data
                        LoadPositionData(null);
                        break;
                }
            }
            catch (Exception ex)
            {
                await new MessageDialog("Something went wrong. Could not get finish current operation.").ShowAsync();
            }
        }
        private void LoadPositionData(Geoposition pos)
        {

            if (pos != null)
            {
                latitude.Text = pos.Coordinate.Point.Position.Latitude.ToString();
                longitude.Text = pos.Coordinate.Point.Position.Longitude.ToString();
                altitude.Text = pos.Coordinate.Point.Position.Altitude.ToString();
            }
            else
            {
                latitude.Text = "00.00";
                longitude.Text = "00.00";
                altitude.Text = "00.00";
            }
        }
    }
}

/* global google */
import {
    default as React,
    Component,
} from "react";
import { compose } from 'recompose';
import { RemoveDuplicateArray,isEmpty }  from '../../../utils/commonUtils';

import {
    withScriptjs,
    withGoogleMap,
    GoogleMap,
    DirectionsRenderer,
    Marker,
    InfoWindow,
    withProps
} from "react-google-maps";
import { commonAPIConfig } from '../../../apiConfig/apiConfig';
export const iconColor={
             Booked:'red',
             Available:'green',
             PTO:'blue',
             TBA:'orange',
             supplier:'purple',
             'Pre-Assignment':'grey' //def 1257
};
export const scheduleStatus={
    Booked:'Confirmed',
    Available:'Available',
    PTO:'PTO',
    TBA:'TBA',  
    'Pre-Assignment': 'Pre-Assignment', //def 1257 
};
const DirectionsServices = compose(
    withScriptjs,
    withGoogleMap)(props => {
const iconUrl = "http://maps.google.com/mapfiles/ms/icons/";
const isShowSupplierInfo = props.markers.filter(x => { return (x.name === null); }).length >0;
return (
        <GoogleMap
            defaultCenter={props.center}
            zoom={3}>
             {             
              !isShowSupplierInfo && <div className="row" id="floating-panel">
                <b className="left mr-2 hide">Resource: </b>
                <select className="col s6" id="start" name="start" disabled={false} onChange={props.handledropDownChange} value={props.selectedTS}>
                    {props.markers && props.markers.map((x,i) => {                       
                        if (x.scheduleStatus === scheduleStatus.Booked || x.scheduleStatus === scheduleStatus.Available || x.scheduleStatus === scheduleStatus.PTO || x.scheduleStatus === scheduleStatus.TBA) {
                            return (
                                <option key={i} value={x.name}>{x.name}</option>
                            );
                        }
                         return null;
                    })}
                </select>               
                <b className="left mr-1">Suppliers: </b>
                <select className="col s8" id="end" name="end" onChange={props.handledropDownChange}  value={props.selectedSupplier}>
                    {props.markers &&  props.markers.map((x,i) => {
                         if (x.scheduleStatus === null) {
                            return (
                                <option key={i} value={x.name}>{x.name}</option>
                            );
                       }
                       return null;
                    })}
                </select>
            </div>
            }
            {props.markers.map((marker, index) => {                                  
                return (
                    <Marker
                        key={index}
                        position={{ lat: marker.location.lat + (index * 0.00002), lng: marker.location.lng + (index * 0.00002) }}
                        onClick={() => props.onMarkerClick(marker)}
                        label={marker.scheduleStatus ? 'TS' : isShowSupplierInfo ? 'WL' : 'S'}
                        options={{ color: '#fff', fontSize: '14px', }}
                        icon={{                              
                            url: marker.scheduleStatus !==null ? iconUrl+iconColor[marker.scheduleStatus]+'.png' : iconUrl+iconColor.supplier+'.png',
                            size: new google.maps.Size(48,40),
                            scaledSize: new google.maps.Size(48, 40)
                               
                        }}
                    >           
                        {marker.showInfo && (
                            <InfoWindow onCloseClick={() => props.onMarkerClose(marker)}>
                                <div>
                                {!isEmpty(props.directions)&&(props.directions !== null && marker.scheduleStatus === (scheduleStatus[marker.scheduleStatus])) &&  <p>{props.distanceInfo.start_address}</p> }
                                
                                { (marker.name === null && marker.scheduleStatus === null) && <p><span style={{ fontWeight:600 }} > Work Location : </span>  {marker.workLocation} </p> }

                                { (marker.name !== null && marker.scheduleStatus === null) && <p><span style={{ fontWeight:600 }} > Supplier : </span>  {marker.name} </p> }
                                { (marker.name !== null && marker.scheduleStatus === null) && <p><span style={{ fontWeight:600 }} > Address : </span>  {marker.workLocation}  </p> }

                                { marker.scheduleStatus !== null && <p> {marker.scheduleStatus !== null && <span>{marker.name}</span> } { marker.scheduleStatus === (scheduleStatus[marker.scheduleStatus]) &&   <span style={{ color:iconColor[marker.scheduleStatus] , fontWeight:600 }}>: {marker.scheduleStatus}</span> } </p> } 

                                {!isEmpty(props.directions) && (props.directions !== null && marker.scheduleStatus === (scheduleStatus[marker.scheduleStatus])) && <div style={{ fontWeight: 600 }}> Selected {isShowSupplierInfo ? 'Location' : 'Supplier'} Distance : {((props.distanceInfo.distance.value) / 1000).toFixed(1) + " km"} / {((props.distanceInfo.distance.value) * 0.000621371).toFixed(1) + " Miles"}. Travel Time : {props.distanceInfo.duration.text}</div>}    { /** def 1384 fix : show distance text in kilometer always */}

                                </div>
                            </InfoWindow>
                        )}
                    </Marker>
                );
            })

            }
            {props.directions && <DirectionsRenderer directions={!isEmpty(props.directions) ? props.directions : null } options={{ suppressMarkers: true,preserveViewport :true }} />}
        </GoogleMap>
    );
});

/*
 * Add <script src="https://maps.googleapis.com/maps/api/js"></script> to your HTML to provide google.maps reference
 */
export default class DirectionsGoogleMap extends Component {
 constructor(props){
     super(props);     
     this.state = {
        selectedTS:{ lat:'',lng:'',name:'' },
        selectedSupplier:{ lat:'',lng:'',name:'' },
        directions: null,
        distanceInfo:null,
        markers:[],
        listofTechnicalSpec:[],
        listofSupplier:[],
        center:{ lat:'',lng:'' } 
    };
  }
    
    handledropDownChange=(e) =>{        
        const selectedName = e.target.value;
       // let selectedLatitude = {};
        this.state.markers.map(x => {
            if (x.name === selectedName) {
                //selectedLatitude = { "location":{ 'lat': x.location.lat, 'lng': x.location.lng,'name':x.name } };
                this.setState({
                    selectedSupplier:{ lat:x.location.lat,lng:x.location.lng,name:x.name },
                    markers: this.state.markers.map((marker,i) => {     
                      return {
                            ...marker,
                            showInfo: false,                        
                        };
                    })
                },()=>{
                    const targetMarker={ lat:0,lng:0 };
                    const shortestLocation={ lat:0, lng:0 };
                    this.getrootMapServices(targetMarker,shortestLocation);
                });
               }
            return null;
        });
       
        // if (e.target.name === 'start') {
        //     this.setState({               
        //         selectedTS:{ lat:selectedLatitude.location.lat,lng:selectedLatitude.location.lng,name:selectedLatitude.name },
        //     });
        // }
        //if (e.target.name === 'end') {           
            // const nearestLocation=[];
            // this.state.markers.map(marker => {
            //     if (marker.scheduleStatus != null) {
            //         nearestLocation.push(this.distanceCalculator(selectedLatitude.location.lng, selectedLatitude.location.lat, marker.location.lng, marker.location.lat,marker.name));
            //     }
            //     return null;
            // });
            //Find Minimum Distance Here...
            // const min = Math.min.apply(null, nearestLocation.map(function (item) {
            //     return item.distance;
            // }));
            // const shortestLocation = nearestLocation.find(obj => obj.distance === min);

            // this.setState({   
            //     selectedTS:{ lat:shortestLocation.lat,lng:shortestLocation.lng,name:shortestLocation.name },           
            //     selectedSupplier:{ lat:selectedLatitude.location.lat,lng:selectedLatitude.location.lng,name:selectedLatitude.location.name },
                
            // },()=>{
            //      //this.OnselectCallback();
            //      });
        //}        
        //if(e.target.name === 'start'){        
            //this.getrootMapServices(selectedLatitude.location,this.state.selectedSupplier);
           // this.showInfoBox(selectedLatitude.location);
       // }
    };

    // OnselectCallback=()=>{
    //     this.getrootMapServices(this.state.selectedTS,this.state.selectedSupplier);
    //     this.showInfoBox(this.state.selectedTS);
    // }

     // show infoBox On Dropdown selection
     showInfoBox=(targetMarker)=>{
        this.setState({
            markers: this.state.markers.map(marker => {
                 if(marker.scheduleStatus !== null){
                    if ((marker.location.lat && marker.location.lng) === (targetMarker.lat && targetMarker.lng)) {
                       
                        return {
                            ...marker,                              
                            showInfo:true
                        };
                    }
                 }
               
                return marker;
            })
           
        }); 
     }

    // Toggle to 'true' to show InfoWindow and re-renders component
    handleMarkerClick =(targetMarker)=> {
        //Onclick Marker show Infobox
        this.setState({
            markers: this.state.markers.map((marker,i) => {                
                if (marker === targetMarker) {
                    return {
                        ...marker,
                        showInfo: !marker.showInfo,                        
                    };
                }else{
                    return {
                        ...marker,
                        showInfo: false,                        
                    };
                }               
            })
           
        });
        if (targetMarker.scheduleStatus !== null) {         
            this.getrootMapServices(targetMarker.location,this.state.selectedSupplier);
            //this.getShortestLocation(targetMarker);
        }
       
    };

    handleMarkerClose=(targetMarker)=> {
        this.setState({
            markers: this.state.markers.map(marker => {
                if (marker === targetMarker) {
                    return {
                        ...marker,
                        showInfo: !marker.showInfo,
                    };
                }
                return marker;
            }),
        });
    };

    // distanceCalculator = (meineLongitude, meineLatitude, long1, lat1, distnationName) => {
    //     const erdRadius = 6371;
    //     const distnation_lng = long1;
    //     const distnation_lat = lat1;
    //     const distnation_Name = distnationName;

    //     meineLongitude = meineLongitude * (Math.PI / 180);
    //     meineLatitude = meineLatitude * (Math.PI / 180);
    //     long1 = long1 * (Math.PI / 180);
    //     lat1 = lat1 * (Math.PI / 180);

    //     const x0 = meineLongitude * erdRadius * Math.cos(meineLatitude);
    //     const y0 = meineLatitude * erdRadius;

    //     const x1 = long1 * erdRadius * Math.cos(lat1);
    //     const y1 = lat1 * erdRadius;

    //     const dx = x0 - x1;
    //     const dy = y0 - y1;

    //     const d = Math.sqrt((dx * dx) + (dy * dy));

    //     const nearestLocationObj = {
    //         "name":distnation_Name,
    //         "lat": distnation_lat,
    //         "lng": distnation_lng,
    //         "distance": d
    //     };
    //     return nearestLocationObj;
    // };

    // getShortestLocation=(targetMarker)=>{  
    //     debugger;      
    //     const nearestLocation = [];
    //     this.state.markers.map(marker => {
    //         if (marker.scheduleStatus == null) {
    //             nearestLocation.push(this.distanceCalculator(targetMarker.location.lng, targetMarker.location.lat, marker.location.lng, marker.location.lat,marker.name));
    //         }
    //         return null;
    //     });
    //     //Find Minimum Distance Here...
    //     const min = Math.min.apply(null, nearestLocation.map(function (item) {
    //         return item.distance;
    //     }));
    //     const shortestLocation = nearestLocation.find(obj => obj.distance === min);
    //     this.setState({           
    //         selectedTS:{ lat:targetMarker.location.lat,lng:targetMarker.location.lng,name:targetMarker.name },
    //         selectedSupplier:{ lat:shortestLocation.lat,lng:shortestLocation.lng,name:shortestLocation.name }
    //     });
    //     debugger;
    //     this.getrootMapServices(targetMarker.location,shortestLocation);
    // };

    // updateDistance=(shortestLocation,targetMarker)=> {
    //         let distance = null;
    //         this.setState({
    //             markers: this.state.markers.map(marker => {
    //                  if(marker.userType === 'TS'){
    //                     if ((marker.location.lat && marker.location.lng) === (targetMarker.lat && targetMarker.lng)) {
    //                         if (shortestLocation.distance < 1) {
    //                             distance = Math.round(shortestLocation.distance * 1000) + " m";
    //                         } else {
    //                             distance = Math.round(shortestLocation.distance * 10) / 10 + " km";
    //                         }
    //                         return {
    //                             ...marker,
    //                             distance: distance,
    //                             showInfo:true
    //                         };
    //                     }
    //                  }
                   
    //                 return marker;
    //             })
               
    //         });   
    // };

    getrootMapServices=(originPath,destinationPath)=>{    
        const DirectionsService = new google.maps.DirectionsService();
        DirectionsService.route({
            origin:{ lat:originPath.lat,lng:originPath.lng },
            destination: { lat:destinationPath.lat,lng:destinationPath.lng },
            travelMode: google.maps.TravelMode.DRIVING,
            unitSystem: google.maps.UnitSystem.METRIC,// def 1384 fix : show distance text in KM/meter always
            optimizeWaypoints: true,
        }, (result, status) => {
            if (status === google.maps.DirectionsStatus.OK) {
                this.setState({
                    directions: result,
                    distanceInfo:result.routes[0].legs[0]
                });
            } else if (status === google.maps.DirectionsStatus.ZERO_RESULTS) {
                this.setState({
                    directions:null,
                    distanceInfo:null
                });
            }else{
                // console.error(`error fetching directions ${ result }`);
            }
        });
    };

    componentDidMount(){      
        const techSpecData =[];
        const supplierData =[];
        const markersData=[];       
        let customObj={};
        let supplierObj={};
        let o;
        this.props.mapData.map((x,i) => { 
                if(x.supplierInfo.length > 0){
                    x.supplierInfo.map((obj,index)=>{                                                                 
                        supplierObj={
                            "supplierName":obj.supplierName,
                            "supplierType":obj.supplierType,
                            "workLocation":x.address, // D - 912
                            "supplierGeoLocation":{
                                "longitude":x.supplierGeoLocation == null ? 0 : x.supplierGeoLocation.longitude, // 404 error for few records so only added for null check in longitude
                                "latitude":x.supplierGeoLocation == null ? 0 : x.supplierGeoLocation.latitude    // 404 error for few records so only added for null check in latitude
                             }                            
                        };
                        // if(obj.supplierName === null){
                        //     supplierObj["workLocation"]=x.location;
                        // }else{
                        //     supplierObj["workLocation"]=null;
                        // };
                        supplierData.push(supplierObj);
                    });
                }            
                if(x.resourceSearchTechspecInfos.length > 0){                 
                    x.resourceSearchTechspecInfos.map((obj,index)=>{
                        techSpecData.push(obj);
                    });
                }  
         });
         const techSpecdata = RemoveDuplicateArray(techSpecData,'epin');
         const supplierdata = RemoveDuplicateArray(supplierData,'supplierName');
         //const techSpecdata = _.uniqBy(techSpecData,'epin');
         //const supplierdata = _.uniqBy(supplierData,'supplierName');

         const finalData = [ ...techSpecdata,  ...supplierdata ];  

         finalData.map((x,i)=>{
               if(x.supplierType === 'Supplier' || x.supplierType === 'SubSupplier' ){                   
                customObj={                   
                    "scheduleStatus": null,
                    "showInfo": false,
                    "name": x.supplierName,
                    "workLocation":x.workLocation,
                    "distanceFromVenderInKm":null,
                    "distanceFromVenderInMile":null,
                    "location": {
                        "lat": x.supplierGeoLocation == null ? 0 : x.supplierGeoLocation.latitude,   // 404 error for few records so only added for null check in longitude
                        "lng": x.supplierGeoLocation == null ? 0 : x.supplierGeoLocation.longitude,  // 404 error for few records so only added for null check in latitude
                    }                   
                };                            

            }else{
                 customObj={
                    "scheduleStatus": x.scheduleStatus,
                    "showInfo": false,
                    "name": x.firstName +' '+ x.lastName,
                    "workLocation":x.workLocation,
                    "distanceFromVenderInKm":x.distanceFromVenderInKm,
                    "distanceFromVenderInMile":x.distanceFromVenderInMile,
                    "location": {
                        "lat": x.techSpecGeoLocation.latitude,
                        "lng": x.techSpecGeoLocation.longitude,
                    }
                };
             }
             markersData.push(customObj);
            
         });        
        this.setState({ selectedTS:{ lat:markersData[0].location.lat, lng:markersData[0].location.lng, name:markersData[0].name },                          
                        selectedSupplier:{ lat:supplierData[0].supplierGeoLocation.latitude,lng:supplierData[0].supplierGeoLocation.longitude,name:supplierData[0].supplierName },
                        listofTechnicalSpec:techSpecdata,
                        listofSupplier:supplierdata,
                        markers:markersData,
                        center:{ lat: markersData[0].location.lat, lng:markersData[0].location.lng }
                    });      
       
    }

    render() {        
        return (
            <DirectionsServices
                googleMapURL={commonAPIConfig.baseUrl + commonAPIConfig.gooogleMap}  //Cyber security issue fix : To Hide google api key in UI
                loadingElement={<div style={{ height: `100%` }} />}
                containerElement={<div style={{ height: `675px` }} />}
                mapElement= {<div style={{ height: `100%` }} />}
                center= {this.state.center}
                directions={this.state.directions}
                distanceInfo={this.state.distanceInfo}
                markers={this.state.markers}
                onMarkerClick={this.handleMarkerClick}
                onMarkerClose={this.handleMarkerClose}
                handledropDownChange={this.handledropDownChange}
                selectedTS={this.state.selectedTS.name}
                selectedSupplier={this.state.selectedSupplier.name}
                listofTechnicalSpec={this.state.listofTechnicalSpec}
                listofSupplier={this.state.listofSupplier}               

            />
        );
    }
}

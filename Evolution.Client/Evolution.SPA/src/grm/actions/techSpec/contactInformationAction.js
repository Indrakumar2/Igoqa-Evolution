import { techSpecActionTypes } from '../../constants/actionTypes';
import { mergeobjects,isEmpty,isUndefined, deepCopy, isEmptyReturnDefault } from '../../../utils/commonUtils';

const actions = {
    UpdateContactInformation: (payload) => ({
        type: techSpecActionTypes.contactInformationActionTypes.UPDATE_CONTACT_INFORMATION,
        data: payload
    }),
    UpdateContact: (payload) => ({
        type: techSpecActionTypes.contactInformationActionTypes.UPDATE_CONTACT,
        data: payload
    }),
    AutoGenerateUserName:(payload)=>({
        type: techSpecActionTypes.contactInformationActionTypes.AUTOGENERATE_USER_NAME,
        data: payload
    }),
    IsRCRMUpdatedContactInformation:(payload)=>({
        type: techSpecActionTypes.contactInformationActionTypes.IS_RCRM_UPDATED_CONTACT,
        data: payload
    })
};
export const UpdateContactInformation = (data) => (dispatch, getstate) => {
    const state = getstate();
    const modifiedData = mergeobjects(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo, data);
    dispatch(actions.UpdateContactInformation(modifiedData));
};

export const IsRCRMUpdatedContactInformation=(data)=> (dispatch)=>{
    dispatch(actions.IsRCRMUpdatedContactInformation(data));
};

export const UpdateContact = (data) => (dispatch, getstate) => {
    const state = getstate();
    const defaultFieldType = state.RootTechSpecReducer.TechSpecDetailReducer.defaultFieldType;
    const epin = state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo.epin;
    const oldData = deepCopy(isEmptyReturnDefault(state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistContact));   
    let count =[];  
        for(let i=0; i<defaultFieldType.length; i++){
               const strState = defaultFieldType[i];
               if(oldData !==undefined && oldData !== null){
                count =  oldData.filter((obj)=>{
                    if(obj.contactType === strState)
                    {                     
                        return obj;
                    } 
                   });
               }              
               if (count.length===0)
               {                
                   if(defaultFieldType[i] === 'PrimaryEmail' && data.emailAddress !== undefined ){
                        oldData.push({ id:Math.floor(Math.random() * (Math.pow(10, 5))),
                                       epin: epin, 
                                       emailAddress: data.emailAddress,
                                       recordStatus: 'N',
                                       contactType : defaultFieldType[i] });                       
                    }
                    if(defaultFieldType[i] === 'SecondaryEmail' && data.secondaryEmailAddress !== undefined ){
                        oldData.push({ id:Math.floor(Math.random() * (Math.pow(10, 5))),
                                       epin: epin, 
                                       emailAddress: data.secondaryEmailAddress,
                                       recordStatus: 'N',
                                       contactType : defaultFieldType[i] });                       
                    }
                   
                    if(defaultFieldType[i] === 'PrimaryPhone' && data.otherTelephoneNumber !== undefined ){
                        oldData.push({ id:Math.floor(Math.random() * (Math.pow(10, 5))),
                                       epin: epin, 
                                       telephoneNumber: data.otherTelephoneNumber,
                                       recordStatus: 'N',
                                       contactType : defaultFieldType[i] });                       
                    }
                    if(defaultFieldType[i] === 'PrimaryMobile' && data.mobileNumber !== undefined ){
                        oldData.push({ id:Math.floor(Math.random() * (Math.pow(10, 5))),
                                       epin: epin, 
                                       mobileNumber: data.mobileNumber,
                                       recordStatus: 'N',
                                       contactType : defaultFieldType[i] });                       
                    }
                    if(defaultFieldType[i] ===  'Emergency' && (data.emergencyContactName !== undefined || data.telephoneNumber !== undefined )){               
                        if(data.emergencyContactName !== undefined){
                            oldData.push({ 
                                id:Math.floor(Math.random() * (Math.pow(10, 5))),
                                epin: epin, 
                                emergencyContactName: data.emergencyContactName,
                                telephoneNumber:null,
                                recordStatus: 'N',
                                contactType : defaultFieldType[i] });
                        }
                        if(data.telephoneNumber !== undefined){
                            oldData.push({ 
                                id:Math.floor(Math.random() * (Math.pow(10, 5))),
                                epin: epin, 
                                telephoneNumber: data.telephoneNumber,
                                recordStatus: 'N',
                                contactType : defaultFieldType[i] });
                        }
                    }
                    if(defaultFieldType[i] === 'PrimaryAddress' && (data.country !== undefined || data.postalCode !== undefined || data.address !== undefined  ))
                        {
                            if(data.country !== undefined)
                            {
                                oldData.push({
                                epin: epin,    
                                id:Math.floor(Math.random() * (Math.pow(10, 5))),
                                country: data.country,
                                countryId:data.countryId, // Added For ITK DEf 1536
                                recordStatus: 'N',
                                contactType : defaultFieldType[i] });
                            }
                            if(data.postalCode !== undefined)
                            {
                                oldData.push({ 
                                id:Math.floor(Math.random() * (Math.pow(10, 5))),
                                epin: epin, 
                                postalCode: data.postalCode,
                                recordStatus: 'N',
                                contactType : defaultFieldType[i] });
                            }
                            if(data.address !== undefined)
                            {
                                oldData.push({ 
                                id:Math.floor(Math.random() * (Math.pow(10, 5))),
                                epin: epin, 
                                postalCode: data.address,
                                recordStatus: 'N',
                                contactType : defaultFieldType[i] });
                            }
                        }                                                 
                }
                else{
                    for(let j=0; j< oldData.length; j++){                       
                        if(oldData[j].contactType === 'PrimaryEmail' && data.emailAddress !== undefined ){
                                oldData[j].emailAddress = data.emailAddress;
                                if (oldData[j].recordStatus !== 'N')
                                        oldData[j].recordStatus = 'M';
                            }                            
                        if(oldData[j].contactType === 'SecondaryEmail' && data.secondaryEmailAddress !== undefined){               
                                oldData[j].emailAddress = data.secondaryEmailAddress;
                                if (oldData[j].recordStatus !== 'N')
                                    oldData[j].recordStatus = 'M';                              
                        }
                        if(oldData[j].contactType === 'PrimaryPhone' && data.otherTelephoneNumber !== undefined){               
                            oldData[j].telephoneNumber = data.otherTelephoneNumber;
                            if (oldData[j].recordStatus !== 'N')
                                oldData[j].recordStatus = 'M';                            
                        }
                        if(oldData[j].contactType === 'PrimaryMobile' && data.mobileNumber !== undefined){               
                            oldData[j].mobileNumber = data.mobileNumber; 
                            if (oldData[j].recordStatus !== 'N')
                                oldData[j].recordStatus = 'M';                          
                        }
                        if(oldData[j].contactType === 'Emergency' && (data.emergencyContactName !== undefined || data.telephoneNumber !== undefined )){               
                            oldData[j].emergencyContactName = data.emergencyContactName !== undefined ? data.emergencyContactName : oldData[j].emergencyContactName;
                            oldData[j].telephoneNumber = data.telephoneNumber !== undefined ? data.telephoneNumber : oldData[j].telephoneNumber;                            
                            if (oldData[j].recordStatus !== 'N')
                                oldData[j].recordStatus = 'M';
                        }
                        if(oldData[j].contactType === 'PrimaryAddress' && (data.country !== undefined || data.county !== undefined || data.city !== undefined || data.postalCode !== undefined || data.address !== undefined  )){               
                            oldData[j].country = data.country !== undefined ? data.country : oldData[j].country;
                            oldData[j].countryId = data.countryId !== undefined ? data.countryId : oldData[j].countryId; // Added For ITK DEf 1536
                            oldData[j].county = data.county !== undefined ? data.county : oldData[j].county;
                            oldData[j].countyId = data.countyId !== undefined ? data.countyId : oldData[j].countyId;// Added For ITK DEf 1536
                            oldData[j].city = data.city !== undefined ? data.city : oldData[j].city;
                            oldData[j].cityId = data.cityId !== undefined ? data.cityId : oldData[j].cityId;// Added For ITK DEf 1536
                            oldData[j].postalCode = data.postalCode !== undefined ? data.postalCode : oldData[j].postalCode; 
                            oldData[j].address = data.address !== undefined ? data.address : oldData[j].address;
                            if (oldData[j].recordStatus !== 'N')
                                oldData[j].recordStatus = 'M';                            
                        }                       
                    }
                }
        } 
    dispatch(actions.UpdateContact(oldData));
};

export const AutoGenerate = (data) => (dispatch, getstate) => {
    const state = getstate();
    const techInfo = (state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo === undefined) ? {} : state.RootTechSpecReducer.TechSpecDetailReducer.selectedProfileDetails.TechnicalSpecialistInfo;
        
    const autoGenerate=[];
    const prefixName=[
        "Karl",
        "Hans",
        "Jacob"
      ];
      const suffixName=[
        "Karlsson",
        "Drageborg",
        "Iversen"
      ];
      const randomName = () => {
        const randomPrefix = Math.floor(Math.random() * (prefixName.length));
        const randomSuffix = Math.floor(Math.random() * (suffixName.length));
        const output = prefixName[randomPrefix]+" "+ suffixName[randomSuffix];
        return output;
      };
      const randomPassword =(length)=>{
        const chars = "abcdefghijklmnopqrstuvwxyz!@#$%^&*()-+<>ABCDEFGHIJKLMNOP1234567890";
        let pass = "";
        for (let x = 0; x < length; x++) {
            const i = Math.floor(Math.random() * chars.length);
            pass += chars.charAt(i);
        }
        return pass;
    }; 
     
    // techInfo['logonName']= randomName();
    techInfo['password']=  randomPassword(10);
dispatch(actions.AutoGenerateUserName(techInfo));
};
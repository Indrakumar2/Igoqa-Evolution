import React, { Component, Fragment } from 'react';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import LabelwithValue from '../../../../common/baseComponents/customLabelwithValue';
import { getlocalizeData, isEmptyOrUndefine } from '../../../../utils/commonUtils';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import MaterializeComponent from 'materialize-css';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { headData } from './headerData';
import CustomModal from '../../../../common/baseComponents/customModal';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import store from '../../../../store/reduxStore';
import Draggable from 'react-draggable'; // The default
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';
const localConstant = getlocalizeData();

class DivisionCostCenterMargin extends Component {
    constructor(props) {
        super(props);
        this.state = {
            //SelectedDivisionName: "Select",
            DivisionAcReference: "",
            isOpen: false,
            //selectedDivisionData: {},
            selectedDivCostCenter:"",
            showDivisionModal:false,
            showCostCentreModal:false,
           
        };
        this.divisionModalRef=React.createRef();
        this.costCenterModal=React.createRef();
        this.divisionRef=React.createRef();
        this.SelectedDivisionName='';
        this.SelectedDivisionValue='';
        this.confirmationModalData = {
            title: "",
            message: "",
            type: "",
            modalClassName: "",
            buttons: []
        };

        this.props.callBackFuncs.onAfterSaveChange = () => {
            // this.setState({
            //     SelectedDivisionName: "Select",
            //     DivisionAcReference: '',
            //     selectedDivisionData: {
            //         companyDivisionId:''
            //     }
            // });
            // this.SelectedDivisionValue = '';
            this.UpdateSelectedDivisionData();
            // document.getElementById('loadedDivision').value = "";
        };
    }

    UpdateSelectedDivisionData =() =>{
        if (this.props.companyDivision.length > 0) {
                this.props.companyDivision.map((iteratedValue) => {
                    if (iteratedValue.divisionName === this.props.selectedDivisionData.divisionName) {
                        this.props.actions.UpdateSelectedDivisionData(iteratedValue);
                    }
                });
            }
            else {
                this.props.actions.UpdateSelectedDivisionData({});
            }
    }
    componentDidMount() {
        const elems = document.querySelectorAll('.modal');
        const instances = MaterializeComponent.Modal.init(elems, { dismissible: false });
        this.props.actions.FetchCompanyDivision();
        document.getElementById("newDivisionName").value = '';
        document.getElementById("newInputReference").value = '';
        //this.setState( { SelectedDivisionName: "Select" } );
    }

    // Commented - Dropdown change issue
//     componentWillReceiveProps(nextProps) 
//     {
    
//         if(nextProps.companyDivision.length>0  && !nextProps.isEditCompanyDivision)
//         {
//             if(parseInt(nextProps.companyDivision[nextProps.companyDivision.length-1].companyDivisionId)>0)
//              {
//                      if(this.state.SelectedDivisionName!="Select" ||this.state.SelectedDivisionName!="")
//                      {
//                         nextProps.companyDivision.map((iteration) => {
                        
//                             if(iteration.divisionName === this.state.SelectedDivisionName)
//                             {
//                                 this.setState( {
//                                     SelectedDivisionName:iteration.divisionName,
//                                     selectedDivisionData: iteration,
//                                     selectedDivCostCenter:""
//                                 });
//                             }
//                         });
//                      }
//                      else
//                      {
//             this.setState(
//                 {
//                     SelectedDivisionName: nextProps.companyDivision[nextProps.companyDivision.length-1].divisionName,
//                     selectedDivisionData: nextProps.companyDivision[nextProps.companyDivision.length-1]
                 
//                 });  
//             }   
//             } 
           
//         }
//   const golbalstate = store.getState();
//         if(golbalstate.CompanyReducer.isbtnDisable && (this.state.selectedDivisionData) )
//         {
//             if(this.state.selectedDivisionData.companyDivisionId<0)
//             {
//             this.setState(
//                 { SelectedDivisionName:"" });
//             }
//         }
//     }
    loadCostCenter = (e) => {
        this.props.actions.FetchDivisionCostCenter();
        this.SelectedDivisionName=e.target.value;
        this.setState({ SelectedDivisionName: e.target.value });
        if ((e.target.value).toLowerCase() === 'select' || e.target.value === "") {
        //     this.setState({ 
        //         DivisionAcReference: '',
        //         selectedDivisionData:{}
        //  });
         this.props.actions.UpdateSelectedDivisionData({});
         //this.SelectedDivisionValue='';
        } else {
            const currentDivsion = this.props.companyDivision && this.props.companyDivision.filter((iteratedValue) => {
                if (iteratedValue.companyDivisionId == e.target.value) {
                    return iteratedValue;
                }
            });
            
            //const divisionData = currentDivsion.length > 0 && currentDivsion[0];
            // this.setState({
            //     selectedDivisionData: divisionData,
            //     DivisionAcReference: divisionData.divisionAcReference,
            //     //SelectedDivisionName: divisionData.divisionName
            // });
            this.props.actions.UpdateSelectedDivisionData(currentDivsion[0]);
            //this.SelectedDivisionValue=divisionData.divisionName;
        }
    }
    handleCreateDivision = () => {
        this.divisionModalRef.current.style.cssText="z-index:1100; display:block";
        this.setState( { showDivisionModal: true  } );
        this.props.actions.UpdateCompanyDivisionButton(false);   
        document.getElementById("newDivisionName").value = '';
        document.getElementById("newInputReference").value = '';
    
    }
    handleEditDivision = (e) => {
        this.divisionModalRef.current.style.cssText="z-index:1100; display:block";
        this.setState( { showDivisionModal: true } );
        this.props.actions.UpdateCompanyDivisionButton(true);
        if(!document.getElementById('loadedDivision').value)
        {
         document.getElementById("newDivisionName").value = '';
         document.getElementById("newInputReference").value = '';
        }
        const oldDivision = this.props.selectedDivisionData.divisionName;
        this.props.companyDivision && this.props.companyDivision.map((iteratedValue) => {
            if (iteratedValue.divisionName === this.props.selectedDivisionData.divisionName) {
                document.getElementById("newDivisionName").value = iteratedValue.divisionName;
                document.getElementById("newInputReference").value = iteratedValue.divisionAcReference;
            }
        });

        if ((oldDivision).toLowerCase() === "select") {
            document.getElementById("newDivisionName").value = '';
            document.getElementById("newInputReference").value = '';
            IntertekToaster(localConstant.companyDetails.division.SELECT_DIVISION_TO_UPDATE,'warningToast oldDivisionWarning');
            return false;
        }
       
    }
    handleNewDivisionCostCentre = () => {
        this.costCenterModal.current.style.cssText="z-index:1100; display:block";
        this.setState( { showCostCentreModal: true  } );
        this.props.actions.UpdateCostCentreButton(false);
      
        document.getElementById('newCostCentreName').value = '';
        document.getElementById('newCostCentreCode').value = '';
    }
    addNewDivision = (e) => {
        e.preventDefault();
       
        this.props.actions.UpdateCompanyDivisionButton(false);
        const divisionName = document.getElementById('newDivisionName').value.trim();
        const divisionReference = document.getElementById('newInputReference').value.trim();
        if (divisionName === "" || divisionName.toLowerCase() === "select") {
            IntertekToaster(localConstant.companyDetails.division.PLEASE_ENTER_DIVISION_NAME,'warningToast emptyDivisionWarning');
            return false;
        }
        if(divisionReference ===""){
            IntertekToaster(localConstant.companyDetails.division.PLEASE_ENTER_ACCOUNT_REFERENCE,'warningToast emptydivisionReferenceWarning');
            return false;
        }
        const isDuplicate = this.props.companyDivision && this.props.companyDivision.find((itertedValue)=>{
            if((itertedValue.divisionName).toUpperCase() === divisionName.toUpperCase() && itertedValue.recordStatus !== "D"){
                return true;
            }
        });
        if (isDuplicate) {
            IntertekToaster(localConstant.companyDetails.division.DIVISION_NAME_ALREADY_EXISTS,'warningToast dupDivisionNameWarning');
            return false;
        }

        const isAccountRefDuplicate = this.props.companyDivision && this.props.companyDivision.find((itertedValue)=>{
            if((itertedValue.divisionAcReference).toUpperCase() === divisionReference.toUpperCase() && itertedValue.recordStatus !== "D"){
                return true;
            }
        });
        if (isAccountRefDuplicate) {
            IntertekToaster(localConstant.companyDetails.division.ACOUNT_REFERENCE_ALREADY_EXISTS,'warningToast dupDivisionNameWarning');
            return false;
        }

        const data = {
            "companyCode": this.props.selectedCompanyCode,
            "divisionName": divisionName,
            "divisionAcReference": divisionReference,
            "recordStatus": "N",
            "modifiedBy": this.props.loginUser,
            "companyDivisionId":Math.floor(Math.random() * 99) -100
        };
        this.props.actions.AddNewDivision(data);
        // this.setState({
        //     selectedDivisionData: data,
        //     //SelectedDivisionName:divisionName,
        //     DivisionAcReference: divisionReference
        // });
        // this.SelectedDivisionValue=divisionName;
        //this.SelectedDivisionName="";
        // this.loadCostCenter(document.getElementById('newDivisionName'));
        this.props.actions.UpdateSelectedDivisionData(data);
        document.getElementById('divisionModalClose').click();   
        this.divisionModalRef.current.style.cssText="display:none";   
       this.setState( { showDivisionModal: false  } );

    }

    cancelDivisionCostCentre = ()=>
    {
        this.divisionModalRef.current.style.cssText="display:none";
        this.setState( { showDivisionModal: false  } );
        this.costCenterModal.current.style.cssText="display:none";
        this.setState( { showCostCentreModal:false } );     
    }
    //To add new cost centre
    addNewCostCentre = (e) => {
        e.preventDefault();
        // this.props.actions.UpdateCostCentreButton(false);
        const divisionCostCentreName = document.getElementById('newCostCentreName').value.trim();
        const divisionCostCentreCode = document.getElementById('newCostCentreCode').value.trim();
        if (divisionCostCentreCode === "") {
            IntertekToaster(localConstant.companyDetails.division.FILL_COST_CENTER_CODE,'warningToast emptydivisionCostCentreCode');
            return false;
        } else if (divisionCostCentreName === "") {
            IntertekToaster(localConstant.companyDetails.division.FILL_COST_CNETER_NAME,'warningToast emptydivisionCostCentreName');
            return false;
        } 

        //D661 - #5 fixes
        // const isDuplicateCode = this.props.companyDivisionCostCenter.find((itertedValue) => {
        //     if ((itertedValue.costCenterCode).toUpperCase() === divisionCostCentreCode.toUpperCase() && itertedValue.companyDivisionId === this.state.selectedDivisionData.companyDivisionId && itertedValue.recordStatus !== "D") {
        //         return true;
        //     }
        // });
        // if (isDuplicateCode) {
        //     IntertekToaster(localConstant.companyDetails.division.COST_CENTER_CODE_EXIST,'warningToast dupCostCentreCode');          
        //     return false;
        // }

        // const isDuplicate = this.props.companyDivisionCostCenter.find((itertedValue) => {
        //     if ((itertedValue.costCenterName).toUpperCase() === divisionCostCentreName.toUpperCase() && itertedValue.companyDivisionId === this.state.selectedDivisionData.companyDivisionId && itertedValue.recordStatus !== "D") {
        //         return true;
        //     }
        // });
        // if (isDuplicate) {
        //     IntertekToaster(localConstant.companyDetails.division.COST_CENTER_NAME_EXISTS,'warningToast dupCostCentreName');
        //     return false;
        // }

        //Scenario - D143(Code and Name duplicate combination)
        const isDuplicate = this.props.companyDivisionCostCenter.find((iteratedValue) => {
            if ((iteratedValue.costCenterCode).toUpperCase() === divisionCostCentreCode.toUpperCase() 
                && (iteratedValue.costCenterName).toUpperCase() === divisionCostCentreName.toUpperCase()
                && iteratedValue.companyDivisionId === this.props.selectedDivisionData.companyDivisionId && iteratedValue.recordStatus !== "D") {
                return true;
              }
        });

        if(isDuplicate){
            IntertekToaster(localConstant.companyDetails.division.COST_CENTER_EXISTS,'warningToast dupCostCentreCode'); 
            return false;
        }

        const data = {
            "companyDivisionCostCenterId": Math.floor(Math.random() * (Math.pow(10, 5))),
            "companyCode": this.props.selectedCompanyCode,
            "division": this.props.selectedDivisionData.divisionName,
            "costCenterCode": divisionCostCentreCode,
            "costCenterName": divisionCostCentreName,
            "recordStatus": "N",
            "modifiedBy": this.props.loginUser,
            "companyDivisionId":this.props.selectedDivisionData.companyDivisionId
        };
        this.props.actions.AddNewDivisionCostCentre(data);
        this.props.actions.FetchDivisionCostCenter(this.props.selectedDivisionData.divisionName);
        this.costCenterModal.current.style.cssText="display:none";
        this.setState( { showCostCentreModal:false } );   
        this.divisionRef.current.click();//company issues
    };
     
    //To update Cost centre
    UpdateDivisionCostCentre = (e) => {
        e.preventDefault();
        const divisionCostCentreName = document.getElementById('newCostCentreName').value.trim();
        const divisionCostCentreCode = document.getElementById('newCostCentreCode').value.trim();
        //const currentDivision = this.state.SelectedDivisionName;
        if (divisionCostCentreName === "") {            
            IntertekToaster(localConstant.companyDetails.division.FILL_COST_CNETER_NAME,'warningToast noDivisionCostCentre');
            return false;
        } else if (divisionCostCentreCode === "") {
            IntertekToaster(localConstant.companyDetails.division.FILL_COST_CENTER_CODE,'warningToast noDivisionCostCentreCode');
            return false;
        }

        //Scenario - D143(Code and Name duplicate combination)
        const isDuplicate = this.props.companyDivisionCostCenter.find((iteratedValue) => {
            if ((iteratedValue.costCenterCode).toUpperCase() === divisionCostCentreCode.toUpperCase()
               && (iteratedValue.costCenterName).toUpperCase() === divisionCostCentreName.toUpperCase()
               && iteratedValue.companyDivisionId === this.props.selectedDivisionData.companyDivisionId && iteratedValue.companyDivisionCostCenterId !== this.props.editedDivisionCostCentre.companyDivisionCostCenterId) {
               return true;
            }
        });

        if(isDuplicate){
            IntertekToaster(localConstant.companyDetails.division.COST_CENTER_EXISTS,'warningToast dupCostCentreCode'); 
            return false;
        }

        //D661 - #5 fixes
        // const isDuplicate = this.props.companyDivisionCostCenter.find((itertedValue) => {
        //     if ((itertedValue.costCenterName).toUpperCase() === divisionCostCentreName.toUpperCase() && itertedValue.companyDivisionId === this.state.selectedDivisionData.companyDivisionId && itertedValue.companyDivisionCostCenterId !== this.props.editedDivisionCostCentre.companyDivisionCostCenterId) {
        //         return true;
        //     }
        // });
        // if (isDuplicate) {
        //     IntertekToaster(localConstant.companyDetails.division.COST_CENTER_NAME_EXISTS,'warningToast dupCostCentreName');
        //     return false;
        // }

        let updatedData;
        if (this.props.editedDivisionCostCentre.recordStatus !== "N") {
            updatedData = {
                oldDataCostcentre: this.props.editedDivisionCostCentre,
                data: {
                    "companyCode": this.props.selectedCompanyCode,
                    "division": this.props.selectedDivisionData.divisionName,
                    "costCenterCode": divisionCostCentreCode,
                    "costCenterName": divisionCostCentreName,
                    "recordStatus": "M",
                    "modifiedBy": this.props.loginUser,
                    "companyDivisionId":this.props.selectedDivisionData.companyDivisionId
                }
            };
        } else {
            updatedData = {
                oldDataCostcentre: this.props.editedDivisionCostCentre,
                data: {
                    "companyCode": this.props.selectedCompanyCode,
                    "division": this.props.selectedDivisionData.divisionName,
                    "costCenterCode": divisionCostCentreCode,
                    "costCenterName": divisionCostCentreName,
                    "recordStatus": "N",
                    "modifiedBy": this.props.loginUser,
                    "companyDivisionId":this.props.selectedDivisionData.companyDivisionId
                }
            };
        }
        this.props.actions.UpdateCompanyDivisionCostcentre(updatedData);
        this.costCenterModal.current.style.cssText="display:none";
        this.setState( { showCostCentreModal:false } );     
        this.divisionRef.current.click();
    }
    deleteDivisionCostCentre = () => {
        const selectedRecords = this.child.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.DIVISION_COST_CENTRE_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: "Yes",
                        onClickHandler: this.deleteSelectedDivisionCostCentre,
                        className: "modal-close m-1 btn-small"
                    },
                    {
                        buttonName: "No",
                        onClickHandler: this.confirmationRejectHandler,
                        className: "modal-close m-1 btn-small"
                    }
                ]
            };
            this.props.actions.DisplayModal(confirmationObject);
        }
        else {            
            IntertekToaster(localConstant.validationMessage.SELECT_ONE_ROW_TO_DELETE,'warningToast noRowSelected');  
        }
    }
    deleteSelectedDivisionCostCentre = () => {
        const selectedRecords = this.child.getSelectedRows();
        this.child.removeSelectedRows(selectedRecords);
        this.props.actions.DeleteCompanyDivisionCostCentre(selectedRecords);
        this.props.actions.HideModal();
        this.forceUpdate();
        // this.setState({ selectedDivisionData : {} });
    }

    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }
    editCompanyDivision = (e) => {
        e.preventDefault();
        // document.getElementById("newDivisionName").value = this.state.selectedDivisionData.divisionName;
        // document.getElementById("newInputReference").value = this.state.selectedDivisionData.divisionAcReference;
      
        const divisionName = document.getElementById("newDivisionName").value;
        const updatedDivisionName = divisionName.trim();
        const updatedReference = document.getElementById("newInputReference").value.trim();
        const isDuplicate = this.props.companyDivision && this.props.companyDivision.find((itertedValue)=>{
            if((itertedValue.divisionName).toUpperCase() === divisionName.toUpperCase()
             && itertedValue.recordStatus !== "D" 
             && itertedValue.companyDivisionId != this.props.selectedDivisionData.companyDivisionId){
                return true;
            }
        });
        if (isDuplicate) {
            IntertekToaster(localConstant.companyDetails.division.DIVISION_NAME_ALREADY_EXISTS,'warningToast dupDivName');
            return false;
        }

        const isAccountRefDuplicate = this.props.companyDivision && this.props.companyDivision.find((itertedValue)=>{
            if((itertedValue.divisionAcReference).toUpperCase() === updatedReference.toUpperCase()
            && itertedValue.recordStatus !== "D"
            && itertedValue.companyDivisionId != this.props.selectedDivisionData.companyDivisionId){
                return true;
            }
        });
        if (isAccountRefDuplicate) {
            IntertekToaster(localConstant.companyDetails.division.ACOUNT_REFERENCE_ALREADY_EXISTS,'warningToast dupDivisionNameWarning');
            return false;
        }
        
        if(updatedReference ===""){
            IntertekToaster(localConstant.companyDetails.division.PLEASE_ENTER_ACCOUNT_REFERENCE,'warningToast noAccRefWarn');
            return false;
        }

        if (updatedDivisionName === '') {            
            IntertekToaster(localConstant.companyDetails.division.PLEASE_ENTER_DIVISION_NAME,'warningToast noDivNameWarn');
            return false;
        }
        // this.setState({
             //SelectedDivisionName: updatedDivisionName,
             //DivisionAcReference: updatedReference
        // });
        let UpdatedCompanyDivisions;
        if (this.props.selectedDivisionData.recordStatus !== "N")
         {
            UpdatedCompanyDivisions = {
                "oldDivisionName": this.props.selectedDivisionData.divisionName,
                data: {
                    "companyCode": this.props.selectedCompanyCode,
                    "divisionName": updatedDivisionName,
                    "divisionAcReference": updatedReference,
                    "recordStatus": "M",
                    "companyDivisionId":this.props.selectedDivisionData.companyDivisionId
                }
            };
        }
        else 
        {
            UpdatedCompanyDivisions = {
                "oldDivisionName": this.props.selectedDivisionData.divisionName,
                data: {
                    "companyCode": this.props.selectedCompanyCode,
                    "divisionName": updatedDivisionName,
                    "divisionAcReference": updatedReference,
                    "recordStatus": "N",
                    "companyDivisionId":this.props.selectedDivisionData.companyDivisionId
                }

            };
        }
        //this.setState({ SelectedDivisionName:updatedDivisionName, DivisionAcReference:updatedReference });
       // this.setState({ selectedDivisionData:UpdatedCompanyDivisions.data });
        this.props.actions.UpdateCompanyDivision(UpdatedCompanyDivisions);
        this.props.actions.UpdateDivisionNameInCostCenter(UpdatedCompanyDivisions.data);
        this.props.actions.UpdateSelectedDivisionData(UpdatedCompanyDivisions.data);

        document.getElementById('divisionModalClose').click();
        //document.getElementById('ModalClose_i').click();
        this.divisionModalRef.current.style.cssText="display:none";
    }
    deleteDivision = () => {
        const isChilExists = this.props.companyDivisionCostCenter.find((itertedValue) => {
            if (itertedValue.recordStatus === "D" && itertedValue.companyDivisionId == this.props.selectedDivisionData.companyDivisionId) {
                return false;
            } else if (itertedValue.recordStatus !== "D" && itertedValue.companyDivisionId ==  this.props.selectedDivisionData.companyDivisionId) {
                return true;
            }
        });
        if (isChilExists) {
            IntertekToaster(localConstant.companyDetails.division.SELECTED_COST_CENTER_ASSOCIATED,'warningToast dataAssociationWarn');
            return false;
        }
        const confirmationObject = {
            title: modalTitleConstant.CONFIRMATION,
            message: modalMessageConstant.DIVISION_MESSAGE,
            type: "confirm",
            modalClassName: "warningToast",
            buttons: [
                {
                    buttonName: "Yes",
                    onClickHandler: this.deleteSelectedDivision,
                    className: "modal-close m-1 btn-small"
                },
                {
                    buttonName: "No",
                    onClickHandler: this.confirmationRejectHandler,
                    className: "modal-close m-1 btn-small"
                }
            ]
        };
        this.props.actions.DisplayModal(confirmationObject);
    }
    deleteSelectedDivision = () => {
        this.setState({ SelectedDivisionName: "Select", DivisionAcReference: "" });
        this.props.actions.DeleteCompanyDivision(this.props.selectedDivisionData.divisionName);
        // this.props.actions.FetchDivisionCostCenter();     
        this.props.actions.HideModal();
        document.getElementById('loadedDivision').value = "";
        //this.SelectedDivisionName="";
        //this.SelectedDivisionValue="";
        this.props.actions.UpdateSelectedDivisionData({});
    }
    formReset = (e) => {
        e.preventDefault();
        document.getElementById("newDivisionName").defaultValue = '';
        document.getElementById("newInputReference").defaultValue = '';
    };

    filterCompanyDivision() {
        const divisionId = this.SelectedDivisionName;
        const divisionName = this.SelectedDivisionValue;
        const divisions = Array.isArray(this.props.companyDivision)?this.props.companyDivision:[];
        let currentDivision = divisions.filter((division) => {
                return division.companyDivisionId === divisionId && division.recordStatus !== "D";
        });
        if(currentDivision.length <= 0){
            currentDivision = divisions.filter((division) => {
            return division.divisionName === divisionName && division.recordStatus !== "D";
         });
        }
        return currentDivision.length>0?currentDivision[0]:{};
    }

    getDivisionAccReference(divisionId)
    {
        const divisions = this.props.companyDivision;
        const selectedDivision = divisions.filter(function(item){ return (item.companyDivisionId == divisionId || item.divisionName == divisionId);});
        if(!selectedDivision || selectedDivision.length <=0)
        return '';
       else
       return selectedDivision[0].divisionAcReference;
    }

    render() {
        const companyDivision = [];
        const companyUpdatedDivision = this.props.companyDivision && this.props.companyDivision.filter((iteratedDivision) => {
            return iteratedDivision.recordStatus !== "D";
        });
        const divisionNameArray = [];
        this.props.divisionNames && this.props.divisionNames.map((iteratedValue) => {
            divisionNameArray.push({ iteratedValue });
        });

        let companyDivisionCostCentre = [];
        companyDivisionCostCentre = this.props.companyDivisionCostCenter && this.props.companyDivisionCostCenter.filter((iteratedValues) => {
             return iteratedValues.companyDivisionId == this.props.selectedDivisionData.companyDivisionId && iteratedValues.recordStatus !== "D";
        });
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        const currentDivision = this.filterCompanyDivision();
        // const ol=Object.keys(currentDivision).length;
        return (

            <Fragment>
                {/* Add cost center modal */}
                <CustomModal modalData={modelData} />
                <Draggable handle=".handle">
                <div id="createCostcenter" className="modal popup-position"  display="none" ref={ this.costCenterModal }>
                    <form id="createCostcenterPopup" className="col s12 p-0">
                        <div className="modal-content">
                            <h6 className="mb-2 ml-0 mr-0 mt-0 handle">{this.props.isEditCompanyDivisionCostCenterUpdate?localConstant.companyDetails.division.EDIT_COST_CENTRE:localConstant.companyDetails.division.ADD_COST_CENTRE_TITLE}
                            <i class={"zmdi zmdi-close right modal-close"} onClick={this.cancelDivisionCostCentre}></i> </h6>
                            <span class="boldBorder"></span>
                            <CustomInput
                                hasLabel={true}
                                label={localConstant.companyDetails.division.COST_CENTER_CODE}
                                labelClass="customLabel mandate"
                                divClassName="m6 s12 pr-0 pl-0 mt-2"
                                maxLength = {fieldLengthConstants.company.divisionCostCenter.COST_CENTER_CODE_MAXLENGTH}
                                type='text'
                                inputName='newCostCentreCode'
                                htmlFor="newCostCentreCode"
                                colSize='m6'
                                inputClass="customInputs"
                                required={true}
                                defaultValue={this.props.editedDivisionCostCentre.costCenterCode}
                            />
                            <CustomInput
                                hasLabel={true}
                                type='text'
                                label={localConstant.companyDetails.division.COST_CENTER_NAME}
                                labelClass="customLabel mandate"
                                divClassName="m6 s12 pr-0 mt-2"
                                name='newCostCentreName'
                                htmlFor="newCostCentreName"
                                colSize='m6'
                                inputClass="customInputs"
                                maxLength = {fieldLengthConstants.company.divisionCostCenter.COST_CENTER_NAME_MAXLENGTH}
                                required={true}
                                defaultValue={this.props.editedDivisionCostCentre.costCenterName}
                            />
                        </div>
                        <div className="modal-footer">
                            <button type="reset" onClick={this.cancelDivisionCostCentre} ref={this.divisionRef} className="modal-close waves-effect waves-teal btn-small mr-2">Cancel</button>
                            {this.props.isEditCompanyDivisionCostCenterUpdate ?
                                <button onClick={this.UpdateDivisionCostCentre} type="submit" className="waves-effect waves-teal btn-small">Submit</button> :
                                <button onClick={this.addNewCostCentre} type="submit" className="waves-effect waves-teal btn-small">Submit</button>
                            }
                        </div>
                    </form>
                </div>
                </Draggable>
                {this.state.showCostCentreModal && <div className="customModalOverlay"></div> }
                <Draggable handle=".handle">
                <div id="create-division" className="modal popup-position" display="none"ref={this.divisionModalRef}>
                    <form id="createDivisionPopup" className="col s12 p-0">
                        <div className="modal-content">
                            <h6 className="ml-0 mr-0 mb-2 mt-0 handle">{this.props.isEditCompanyDivision?localConstant.companyDetails.division.EDIT_DIVISION:localConstant.companyDetails.division.ADD_DICISION_COST_CENTRES}
                            <i class={"zmdi zmdi-close right modal-close"} onClick={this.cancelDivisionCostCentre}></i></h6>
                            <span class="boldBorder"></span>
                            <CustomInput
                                hasLabel={true}
                                label={localConstant.companyDetails.division.DIVISION}
                                divClassName="m6 s12 pr-0 pl-0 mt-2"
                                labelClass="customLabel mandate"
                                type='text'
                                inputName='Division'
                                htmlFor="newDivisionName"
                                colSize='m6'
                                inputClass="customInputs"
                                maxLength = {fieldLengthConstants.company.divisionCostCenter.DIVISION_MAXLENGTH}
                                required={true}
                            />
                            <CustomInput
                                hasLabel={true}
                                label={localConstant.companyDetails.division.ACOUNT_REFERENCE}
                                divClassName="m6 s12 pr-0 mt-2"
                                labelClass="customLabel mandate"
                                type='text'
                                inputName='InputReference'
                                htmlFor="newInputReference"
                                colSize='m6'
                                inputClass="customInputs"
                                required={true}
                                maxLength={fieldLengthConstants.company.divisionCostCenter.ACOUNT_REFERENCE_MAXLENGTH}
                            />
                        </div>
                        <div className="modal-footer">
                            <button type="button" onClick={this.cancelDivisionCostCentre}  id="divisionModalClose" className="modal-close waves-effect waves-teal btn-small mr-2">Cancel</button>
                            {this.props.isEditCompanyDivision ?
                                <button onClick={this.editCompanyDivision} className="waves-effect waves-teal btn-small mr-2">Submit</button> :
                                <button onClick={this.addNewDivision} className="waves-effect waves-teal btn-small">Submit</button>
                            }
                        </div>
                    </form>
                </div>
                </Draggable>
                {this.state.showDivisionModal && <div className="customModalOverlay"></div> }

                <div className="genralDetailContainer customCard mt-0">
                    {/* <div className="row">
                        <h6 className="bold pl-0">{localConstant.companyDetails.division.DIVISION_COST_CENTER_MARGIN}</h6>
                        <div className="right-align danger-txt col s12 m6 pt-2 pr-4">*{localConstant.validationMessage.ALL_FIELDS_ARE_MANDATORY}</div>
                    </div> */}
                    <p><span className='pl-0 bold'>{localConstant.companyDetails.division.DIVISION_COST_CENTER_MARGIN}</span></p>
                    <div className="row">
                        <CustomInput
                            hasLabel={true}
                            divClassName='col loadedDivision'
                            label='Division'
                            type='select'
                            colSize='s3'
                            className="browser-default customInputs"
                            optionsList={companyUpdatedDivision}
                            optionName='divisionName'
                            optionValue="companyDivisionId"
                            id="loadedDivision"
                            onSelectChange={(e) => this.loadCostCenter(e)}
                            defaultValue={this.props.selectedDivisionData.companyDivisionId}
                         
                        />
                       {this.props.pageMode!==localConstant.commonConstants.VIEW && <div className="col s3 mt-4x">
                            <button  onClick={this.handleCreateDivision} className="mr-1 modal-trigger btn-small btn-primary ">{localConstant.companyDetails.division.ASSIGN_DIVISION}</button>
                            <button onClick={this.handleEditDivision} disabled={isEmptyOrUndefine(this.props.selectedDivisionData.divisionName)} className="mr-1 btn-small btn-primary editTxtColor waves-effect modal-trigger ">{localConstant.companyDetails.common.EDIT}</button>
                            <button className="btn-small btn-primary mr-1 dangerBtn modal-trigger waves-effect "  onClick={this.deleteDivision} disabled={isEmptyOrUndefine(this.props.selectedDivisionData.divisionName)}>{localConstant.companyDetails.common.DELETE}</button>
                        </div>}
                    </div>
                    <div className="row mb-0">
                        <LabelwithValue
                            colSize="s3"
                            label="Accounts Reference:"
                            value={this.getDivisionAccReference(this.props.selectedDivisionData.companyDivisionId)}
                            />
                            <div className="col s12">
                             <ReactGrid gridRowData={companyDivisionCostCentre} gridColData={headData} onRef={ref => { this.child = ref; }} paginationPrefixId={localConstant.paginationPrefixIds.divisionCostCenter}/>
                                
                                {this.props.pageMode!==localConstant.commonConstants.VIEW &&  <div className="right-align mt-2">
                                    <button onClick={this.handleNewDivisionCostCentre} className="modal-trigger waves-effect btn-small" disabled={isEmptyOrUndefine(this.props.selectedDivisionData.divisionName)}>{localConstant.companyDetails.division.ADD_COST_CENTRE}</button>
                                    <button onClick={this.deleteDivisionCostCentre} className="ml-2 btn-small waves-effect dangerBtn  modal-trigger" disabled={isEmptyOrUndefine(this.props.selectedDivisionData.divisionName)}>{localConstant.companyDetails.common.DELETE}</button>
                                </div>}
                           
                        </div>
                    </div>
                </div>
            </Fragment>

        );
    }
}
export default DivisionCostCenterMargin;
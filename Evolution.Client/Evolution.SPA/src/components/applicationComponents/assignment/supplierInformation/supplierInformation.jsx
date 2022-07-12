import React, { Component, Fragment } from 'react'; 
import { Link } from 'react-router-dom';
import { AppMainRoutes } from '../../../../routes/routesConfig';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import { getlocalizeData, formInputChangeHandler, bindAction,isEmpty,isEmptyReturnDefault,deepCopy, getNestedObject, ObjectIntoQuerySting } from '../../../../utils/commonUtils';
import LabelWithValue from '../../../../common/baseComponents/customLabelwithValue';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './headerData';
import Modal from '../../../../common/baseComponents/modal';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { modalTitleConstant,modalMessageConstant } from '../../../../constants/modalConstants';
import CustomModal from '../../../../common/baseComponents/customModal';
import { required, requiredNumeric } from '../../../../utils/validator';
import SupplierAnchor from '../../../viewComponents/supplier/supplierAnchor';
import arrayUtil from '../../../../utils/arrayUtil';

const localConstant=getlocalizeData();

/** Main Supplier Section */
const MainSupplier = (props) => {
    return(
        <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.assignments.MAIN_SUPPLIER} colSize="s12">
            <div className="row">
                <div className="col s12 m7">
                    <LabelWithValue
                        className="custom-Badge col br1"
                        colSize="s12 m12 mt-4"
                        label={`${ localConstant.assignments.SUPPLIER_NAME }: `}
                         value={props.assignmentDetails.assignmentSupplierName}
                    />
                    <div className="row ml-3">
                        {/* TO-DO:Have to implement Supplier anchor */}
                        <a  href='javascript:void(0)' onClick={props.selectedRowHandler} className={props.assignmentDetails.assignmentSupplierId ? "link" : "link isDisabled waves-effect"}>{ localConstant.assignments.VIEW_SUPPLIER }</a>
                        {/* <Link target='_blank' to={{ pathname: AppMainRoutes.supplierDetail, search: `?supplierId=${ props.assignmentDetails.assignmentSupplierId }` }} className={props.assignmentDetails.assignmentSupplierId ? "link" : "link isDisabled waves-effect"}>{localConstant.assignments.VIEW_SUPPLIER}</Link> */}
                    </div>
                </div>
                    <CustomInput
                    hasLabel={true}
                    divClassName='col'
                    label={localConstant.assignments.SUPPLIER_CONTACT}
                    type='select'
                    colSize='s12 m3'
                    className="browser-default"
                    optionsList={props.supplierContacts}
                    // labelClass={props.supplierContactsRequired ? "customLabel mandate" :"customLabel" }
                    labelClass="mandate"
                    optionName='supplierContactName'
                    optionValue="supplierContactName"
                    name="mainSupplierContactName"
                    id="supplierContactId"
                    disabled={props.interactionMode}
                    defaultValue={props.assignmentSubSupplier.mainSupplierContactName}
                    onSelectChange={props.onChangeHandler}
                    // required={props.supplierContactsRequired}
                    required={true}
                />

    { !props.isResourceAssigned ?
                    <CustomInput
                    type='switch'
                    switchLabel={localConstant.assignments.FIRST_VISIT}
                    isSwitchLabel={true}
                    switchName="isMainSupplierFirstVisit"
                    id="isMainSupplierFirstVisitId"
                    colSize="s12 m2"
                    disabled={props.interactionMode}
                    checkedStatus={props.assignmentSubSupplier.isMainSupplierFirstVisit}
                    onChangeToggle={props.onChangeHandler}
                    refProps ={props.switchRefProps}
                    switchKey={props.assignmentSubSupplier.isMainSupplierFirstVisit? true : false}
                />:
                null
                }
            </div>    
        </CardPanel>        
    );
};

/** Popup for Sub-Suppliers*/
const SubSupplierPopup = (props) =>{
    return (
        <Fragment>
            <div className="row">
                <LabelWithValue
                    className="custom-Badge col br1"
                    colSize="s12 m4 mt-4"
                    label={`${ localConstant.assignments.SUB_SUPPLIER }: `}
                    value={props.subsupplierName}
                />
                 <CustomInput
                    hasLabel={true}
                    divClassName='col'
                    label={localConstant.assignments.SUPPLIER_CONTACT}
                    type='select'
                    colSize='s12 l6'
                    className="browser-default"
                    optionsList={props.subsupplierContacts}
                    labelClass={props.subsupplierContactsRequired ? "customLabel mandate" :"customLabel" }
                    optionName='supplierContactName'
                    optionValue="supplierContactName"
                    name="supplierContactName"
                    id="subsupplierContactId"
                    defaultValue={props.subSupplierRowData.supplierContactName}
                    onSelectChange={props.SubSupplierOnChangeHandler}
                    required={props.subsupplierContactsRequired}
                />
            </div>
            <div className="row">
                 <CustomInput
                    type='switch'
                    switchLabel={localConstant.assignments.PART_OF_ASSIGNMENT}
                    isSwitchLabel={true}
                    switchName="isPartofAssignment"
                    id="isPartofAssignmentId"
                    colSize="s12 m2 l6"
                    checkedStatus={props.subSupplierRowData.isPartofAssignment}
                    onChangeToggle={props.SubSupplierOnChangeHandler}
                />
                 <CustomInput
                    type='switch'
                    switchLabel={localConstant.assignments.FIRST_VISIT}
                    isSwitchLabel={true}
                    switchName="isSubSupplierFirstVisit"
                    id="isSubSupplierFirstVisitId"
                    colSize="s12 m2 l6"
                    checkedStatus={props.subSupplierRowData.isSubSupplierFirstVisit}
                    onChangeToggle={props.SubSupplierOnChangeHandler}
                    disabled={props.currentPage===localConstant.assignments.EDIT_VIEW_ASSIGNMENT_CURRENTPAGE?true:false}
                />
            </div>
        </Fragment>
    );
};

class SupplierInformation extends Component {
    constructor(props){
        super(props);
        this.updatedData={};
        this.editedRowData={};
        this.filterdAssignmentSubSupplier=[];
        this.selectedSubSupplierId="";
        this.state={
            isShowSubSupplierModal:false,
            mainSupplierData:{},
            isMainSupplierFirstVisit:false,
            subSupplierData:{},
            interactionMode:false,
            isShowMainSupplierModal:false,
            subSupplierSelected:false,
        };
        this.switchRef= React.createRef();
        this.onTechSpecRowSelect = this.onTechSpecRowSelect.bind(this);
        this.onSupsupplierRowSelect = this.onSupsupplierRowSelect.bind(this);
        this.props.callBackFuncs.onSave =()=>{
            this.clearSupplierInformationLocalData();
        };
        this.props.callBackFuncs.onCancel=()=>{
            this.clearSupplierInformationLocalData();
        };
    }

    /** Clear Supplier Information Local Data */
    clearSupplierInformationLocalData = () => {
        this.editedRowData = {};
    };

    disableEditColumn = (e) => {
        return (this.props.isOperatorCompany && this.props.isInterCompanyAssignment) ? true : false;
    }

    /**Sub-Supplier Grid Row Selection  */
    onSupsupplierRowSelect = (e) => {
        let assignedSubsupplierTS = [];
        if (e.node.selected) {
            //this.setState({ subSupplierSelected : true });
            this.subSupplierSelectedTest = false;

            this.editedRowData = e.data;
            // const techspec = this.props.techSpec;
            // techspec.forEach(ItratedValuetechspech => {
            //     ItratedValuetechspech.isAssignedToThisSubSupplier = false;
            // });

            const res =deepCopy(this.child.getAllRows()); //D356
            res.forEach(ItratedValuetechspech => {
                ItratedValuetechspech.isAssignedToThisSubSupplier = false; //Changes for D1315
            });
            const assignmentSubSupplier = this.props.assignmentSubSupplier;
            //    console.log("Node Selected = ", this.editedRowData);
            this.currentSelectedSubSupplier = this.editedRowData;
            // Instead of subSupplierSupplierId, check it with subSupplierId
            const indexSubSupplier = assignmentSubSupplier.findIndex(value => (value.subSupplierId === this.editedRowData.supplierId)); //MS-TS Link CR
            if (indexSubSupplier >= 0) {
                assignedSubsupplierTS = assignmentSubSupplier[indexSubSupplier].assignmentSubSupplierTS.filter(x=>x.isAssignedToThisSubSupplier!=false && x.recordStatus !="D" && x.isDeleted != true);//MS-TS
            }
            if (assignedSubsupplierTS) {
                if (res.length === assignedSubsupplierTS.length) {
                    res.forEach(ItratedValuetechspech => {
                        ItratedValuetechspech.isAssignedToThisSubSupplier = true;
                    });
                }

                else if (assignedSubsupplierTS.length === 0) {
                    res.forEach(ItratedValuetechspech => {
                        ItratedValuetechspech.isAssignedToThisSubSupplier = false;
                    });
                }
                else {
                    assignedSubsupplierTS.forEach(element => {
                        const index = res.findIndex(row => (row.epin === element.epin));
                        if (index >= 0) {
                            res[index].isAssignedToThisSubSupplier = true;
                        }
                    });
                }
                this.props.actions.FetchAssignmentTechSpec(res);
            } 
        }
        else{
            //this.setState({ subSupplierSelected : false });
            // console.log("Node De Selected = ", );
            const techspec=this.props.techSpec;
            techspec.forEach(ItratedValuetechspech =>{
                    ItratedValuetechspech.isAssignedToThisSubSupplier=false;
            });
            
            if(e.data.supplierId === this.currentSelectedSubSupplier.supplierId){
                this.editedRowData=this.currentSelectedSubSupplier;
                this.props.actions.FetchAssignmentTechSpec(techspec);
            }else{
                this.props.actions.FetchAssignmentTechSpec(techspec);
           const res=deepCopy(this.child.getAllRows());//D356
           const assignmentSubSupplier=this.props.assignmentSubSupplier;
        //    console.log("Node Selected = ", this.editedRowData);
           this.currentSelectedSubSupplier = this.editedRowData;
           // Instead of subSupplierSupplierId, check it with subSupplierId
           const indexSubSupplier=assignmentSubSupplier.findIndex(value => (value.subSupplierId === this.editedRowData.supplierId)); //MS-TS Link CR
           if(indexSubSupplier >= 0){
                assignedSubsupplierTS=assignmentSubSupplier[indexSubSupplier].assignmentSubSupplierTS.filter(x=>x.isAssignedToThisSubSupplier!=false && x.recordStatus !="D" && x.isDeleted != true);//MS-TS
           }
            if (assignedSubsupplierTS) {
                if (res.length === assignedSubsupplierTS.length) {
                    res.forEach(ItratedValuetechspech => {
                        ItratedValuetechspech.isAssignedToThisSubSupplier = true;
                    });
                }

                else if (assignedSubsupplierTS.length === 0) {
                    res.forEach(ItratedValuetechspech => {
                        ItratedValuetechspech.isAssignedToThisSubSupplier = false;
                    });
                }
                else {
                    assignedSubsupplierTS.forEach(element => {
                        const index = res.findIndex(row => (row.epin === element.epin));
                        if (index >= 0) {
                            res[index].isAssignedToThisSubSupplier = true;
                        }
                    });
                }
                this.props.actions.FetchAssignmentTechSpec(res);
            } 
        }
    }
    }

    onTechSpecRowSelect = (e) => {
        if (!this.props.isArsAssignment) {
            if (isEmpty(this.editedRowData)) {
                IntertekToaster(localConstant.warningMessages.CHOOSE_ANYONE_OF_SUBSUPPLIER, 'warningToast chooseanyonesubsupplierscenario');
                return false;
            }
            if (!this.editedRowData.isPartofAssignment) {
                IntertekToaster(localConstant.warningMessages.CHOOSE_ANYONE_OF_SUBSUPPLIER_WITH_PARTOFASSIGNMENT, 'warningToast chooseanyonesubsupplierscenario');
                return false;
            }
            if(e.node.selected){
                const selectedRecords = {};
                if (this.props.currentPage === localConstant.assignments.ADD_ASSIGNMENT_CURRENTPAGE) {
                    selectedRecords.assignmentSubSupplierTSId = null;
                    selectedRecords.assignmentSubSupplierId = null;
                }
                else {
                    selectedRecords.assignmentSubSupplierTSId = e.data.assignmentSubSupplierTSId ? e.data.assignmentSubSupplierTSId : null;
                    selectedRecords.assignmentSubSupplierId = this.editedRowData.assignmentSubSupplierId;
                }
                selectedRecords.epin = e.data.epin;
                selectedRecords.isAssignedToThisSubSupplier = e.node.selected;
                this.props.actions.AddSupSupplierTechSpec(selectedRecords, this.editedRowData.supplierId);
            } else{
                this.props.actions.deleteNDTSubSupplierTS(e.data,this.editedRowData.supplierId);
            }
        }
    }

    editRowHandler = (data) => {
        this.setState({
            isShowSubSupplierModal: true,
        });
        this.editedRowData = data;
        this.props.actions.FetchSubsupplierContacts(data.supplierId);
    }

    componentDidMount(){  
      const selectedsupplierPOId=this.props.assignmentDetails.assignmentSupplierPurchaseOrderId;
        if(this.props.isOperatorCompany && this.props.isInterCompanyAssignment){
            this.setState({
                interactionMode:true
            });
        }
        if(selectedsupplierPOId === undefined || required(selectedsupplierPOId)){
            this.setState({
                interactionMode:true
            });
        } else{
            if(this.props.assignmentSubSupplier && this.props.assignmentSubSupplier.length > 0){
                this.setState({ mainSupplierData:this.props.assignmentSubSupplier[0] });
            }
        }
        this.updatedData={};
        this.props.actions.FetchAssignmentVisits();
        this.props.actions.FetchAssignmentTechSpec(this.props.techSpec); 
    }

    componentWillUnmount(){  
        const res = this.props.techSpec;
        res.forEach(ItratedValuetechspech => {
            ItratedValuetechspech.isAssignedToThisSubSupplier = false; //Changes for D1315
        });
        this.props.actions.FetchAssignmentTechSpec(res); 
    }

    onChangeHandler=(e)=>{
        const result = formInputChangeHandler(e);
        this.updatedData[result.name] = result.value;
        if(result.name === 'mainSupplierContactName'){
            const selectedIndex= e.target.selectedIndex;
            const supplierContactsIndex=selectedIndex - 1;
            if(supplierContactsIndex >= 0){
                const supplierContact=this.props.supplierContacts[supplierContactsIndex];
                // const iteratedsubsupplierContacts=Object.assign({},supplierContactId, this.props.supplierContacts[supplierContactsIndex]);
                this.updatedData['mainSupplierContactId']=supplierContact.supplierContactId;
            }
            else{
                this.updatedData['mainSupplierContactId']=null;
            }
        }

        //let hasMoreFirstVisit = false;
        if(this.props.subsuppliers){
           this.props.subsuppliers.forEach(iteratedValue =>{
                if(this.updatedData.isMainSupplierFirstVisit && iteratedValue.isSubSupplierFirstVisit){
                   IntertekToaster(localConstant.warningMessages.ONLY_ONE_SUPPLIER_lOCATION_ALLOWED, 'warningToast onlyonesupplierReqFirstscenario');
                   this.updatedData.isMainSupplierFirstVisit=false;
                    this.switchRef.current.checked=false;
                   //  hasMoreFirstVisit = true;
                }                           
           });
       }
      // if(!hasMoreFirstVisit){
           this.props.actions.UpdateMainSupplierInfo(this.updatedData);
           this.setState({ mainSupplierData: Object.assign({},this.filterdAssignmentSubSupplier[0],this.updatedData) });
           // const editedData=Object.assign({},this.props.assignmentSubSupplier[0],this.updatedData);
           // this.props.actions.AddSubSupplierInformation(editedData);
    //   }

        this.setState({
            mainSupplierData:this.updatedData
        });

         /** validate Only one Supplier is first visit started */
         if(this.props.subsuppliers){
            this.props.subsuppliers.forEach(iteratedValue =>{
                 if(this.updatedData.isMainSupplierFirstVisit && iteratedValue.isSubSupplierFirstVisit){
                    IntertekToaster(localConstant.warningMessages.ONLY_ONE_SUPPLIER_lOCATION_ALLOWED, 'warningToast onlyonesupplierReqFirstscenario');
                    return false;
                 }                           
            });
        }
     /** validate Only one Supplier is first visit ended */
       
    }

    selectedRowHandler = () => {
        const redirectionURL = AppMainRoutes.supplierDetail;
        const supplierId = this.props.assignmentDetails.assignmentSupplierId;
        const queryObj={
            supplierId:supplierId && supplierId
        };
        const queryStr = ObjectIntoQuerySting(queryObj);
        window.open(redirectionURL + '?'+queryStr,'_blank');
    }

    onMainSupplierPopupShowHandler =(e)=>{
        e.preventDefault();
        this.setState({
            isShowMainSupplierModal: true,
        });
    }

    SubSupplierOnChangeHandler=(e)=>{
   
        const result = formInputChangeHandler(e);
        this.updatedData[result.name] = result.value;
        if(result.name === 'supplierContactName'){
            const selectedIndex= e.target.selectedIndex;
            const subsupplierContactsIndex=selectedIndex - 1;
            if(subsupplierContactsIndex >= 0){
                const subsupplierContact=this.props.subsupplierContacts[subsupplierContactsIndex];
                // const iteratedsubsupplierContacts=Object.assign({},subsupplierContactsId, this.props.subsupplierContacts[subsupplierContactsIndex]);
                this.updatedData['supplierContactId']=subsupplierContact.supplierContactId; 
            }
            else{
                this.updatedData['supplierContactId']=null; 
            }
        }
        this.setState({
            subSupplierData:this.updatedData
        });

        if(this.state.mainSupplierData.isMainSupplierFirstVisit && this.updatedData.isSubSupplierFirstVisit ){
            IntertekToaster(localConstant.warningMessages.ONLY_ONE_SUPPLIER_lOCATION_ALLOWED, 'warningToast onlyonesupplierReqSecondscenario');
            return false;
        }
    }

    /** sub-supplier Contact change handler
     * get selected row data
     * update the contact id and name in this.updatedData
     * if isPartOfAssignment
     *      Action 1 : update AssignmentSubSupplier in AssignmentDetail (record status as N OR M)
     *      Action 2 : update subSupplier Array used for Grid (without record status)
     * else
     *      Toaster validation to select Part Of Assignment.
     */

    subSupplierContactChangeHandler = async (data,e) => {
        const result = formInputChangeHandler(e);
        this.updatedData.subSupplierContactId = requiredNumeric(result.value) ? null : parseInt(result.value);
        this.updatedData.subSupplierContactName = e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text !== localConstant.commonConstants.SELECT ? e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text : "";
        if(data && data.isPartofAssignment){
            const response = await this.props.actions.updateSubSupplierContact(data,this.updatedData);
            if(response)
                this.supplierChild.refreshGrid();
        } else {
            // sub supplier should be part of assignment.
        }
        this.updatedData = {};
    };

/** To update supplier Contact name and sub  supplier contact name based validations  */
    supplierContactNameHandler=(data,e)=>{
        this.editedRowData=data;
            this.updatedData['supplierContactId']=parseInt(e.target.value); 
            this.updatedData['supplierContactName']=e.nativeEvent.target[e.nativeEvent.target.selectedIndex].text;
                
         this.setState({
            subSupplierData:this.updatedData
        });
        const editedData=Object.assign({},this.editedRowData,this.updatedData);

        if(editedData.supplierContactName==='Select'){
                            
            editedData.supplierContactId="";
            editedData.supplierContactName="";
           }

           if(editedData.supplierContactName==='Select'){
                            
              editedData.supplierContactValidation=localConstant.commonConstants.supplierContactValidation;
           }
           else{
              editedData.supplierContactValidation="";
           }
           if(this.filterdAssignmentSubSupplier && this.filterdAssignmentSubSupplier.length > 0){
            editedData.mainSupplierContactName = this.filterdAssignmentSubSupplier[0].mainSupplierContactName;
            editedData.mainSupplierContactId = this.filterdAssignmentSubSupplier[0].mainSupplierContactId;
            editedData.isMainSupplierFirstVisit = this.filterdAssignmentSubSupplier[0].isMainSupplierFirstVisit;
        }
           if(this.props.currentPage === 'editViewAssignment'){
      
            if(editedData.assignmentSubSupplierId){
                if(editedData.recordStatus !==  'N'){
                    editedData.recordStatus = 'M';
                }
             /** validate Only one Supplier is first visit ended */
             this.props.actions.UpdateSupplierInformation(editedData,this.editedRowData);
            }
            else{
                editedData.recordStatus = 'N';
                editedData.assignmentId = this.props.assignmentDetails.assignmentId;
                 /** validate Only one Supplier is first visit started */

             /** validate Only one Supplier is first visit ended */
             this.props.actions.AddSubSupplierInformation(editedData,this.editedRowData);
            }
        }
         else{
            editedData.recordStatus = "N";
         this.props.actions.UpdateSubSuppliers(editedData).then(res=>{
             if(res){
                 this.supplierChild.refreshGrid();
             }
         });
            
              this.props.actions.AddSubSupplierInformation(editedData,this.editedRowData);  
            }
       
             this.updatedData={};  
    
    }

    /** sub-supplier First Visit Handler
     * get selected row data
     * update the isSubSuppilerFirstVisit in this.updatedData
     * if isPartOfAssignment
     *      if isSubSupplierFirstVisit
     *          validate for any other first visit is there.
     *          if no other first visit
     *              Action 1 : update AssignmentSubSupplier in AssignmentDetail (record status as N OR M)
     *              Action 2 : update subSupplier Array used for Grid (without record status)
     *          else 
     *              throw validation toaster.
     *      else
     *          Action 1 : update AssignmentSubSupplier in AssignmentDetail (record status as N OR M)
     *          Action 2 : update subSupplier Array used for Grid (without record status)
     * else
     *      throw validation toaster.
     */

    firstVisitChangeHandler = async (e,data) => {
        const assignmentSubSupplier = this.props.assignmentSubSupplier.filter(x => x.recordStatus !== 'D');
        const isSupplierHasFirstVisit = assignmentSubSupplier.filter(x => (x.isSubSupplierFirstVisit === true || x.isMainSupplierFirstVisit === true)).length > 0;
        const result = formInputChangeHandler(e);
        this.updatedData[result.name] = result.value;
        if(data.isPartofAssignment){
            if(result.value && isSupplierHasFirstVisit){
                IntertekToaster(localConstant.warningMessages.ONLY_ONE_SUPPLIER_lOCATION_ALLOWED, 'warningToast onlyonesupplierReqThirdscenario');
                this.supplierChild.refreshGrid();
                this.updatedData = {};
                return false;
            }
            const response = await this.props.actions.updateSubSupplierFirstVisit(data,this.updatedData);
            if(response)
                this.supplierChild.refreshGrid();
        } else {
            // sub supplier should be part of assignment.
        }
        this.updatedData = {};
    };

    /** To updated First visit of subsuppliers */
    firstVisitHandler=(e,data)=>
    {
        const result = formInputChangeHandler(e);
        this.editedRowData=data;
        this.updatedData[result.name] = result.value;

        const editedData=Object.assign({},this.editedRowData,this.updatedData);
        if(this.filterdAssignmentSubSupplier && this.filterdAssignmentSubSupplier.length > 0){
            editedData.mainSupplierContactName = this.filterdAssignmentSubSupplier[0].mainSupplierContactName;
            editedData.mainSupplierContactId = this.filterdAssignmentSubSupplier[0].mainSupplierContactId;
            editedData.isMainSupplierFirstVisit = this.filterdAssignmentSubSupplier[0].isMainSupplierFirstVisit;
        }
        if(this.props.currentPage === 'editViewAssignment'){
   
            if(editedData.assignmentSubSupplierId){
                if(editedData.recordStatus !==  'N'){
                    editedData.recordStatus = 'M';
                }
                 /** validate Only one Supplier is first visit started */
                 if(this.props.subsuppliers){
                    this.props.subsuppliers.forEach(iteratedValue =>{
                         if(editedData.isSubSupplierFirstVisit && iteratedValue.isSubSupplierFirstVisit){
                             iteratedValue.isSubSupplierFirstVisit=false;
                             const iteratedAssignment =Object.assign({}, this.props.assignmentSubSupplier[iteratedValue.subSupplierId], iteratedValue);
                             iteratedAssignment.mainSupplierContactName = this.filterdAssignmentSubSupplier[0].mainSupplierContactName;//MSTS
                             iteratedAssignment.mainSupplierContactId = this.filterdAssignmentSubSupplier[0].mainSupplierContactId;//MSTS
                             iteratedAssignment.isMainSupplierFirstVisit = this.filterdAssignmentSubSupplier[0].isMainSupplierFirstVisit;//MSTS
                             this.props.actions.UpdateSupplierInformation(iteratedAssignment,iteratedValue);
                         }                           
                    });
                }
             /** validate Only one Supplier is first visit ended */
             this.props.actions.UpdateSubSuppliers(editedData).then(res=>{
                if(res){
                    this.supplierChild.refreshGrid();
                }
            });
             this.props.actions.UpdateSupplierInformation(editedData,this.editedRowData);
            }
            else{
                editedData.recordStatus = 'N';
                editedData.assignmentId = this.props.assignmentDetails.assignmentId;
                 /** validate Only one Supplier is first visit started */
                 if(this.props.subsuppliers){
                    this.props.subsuppliers.forEach(iteratedValue =>{
                         if(editedData.isSubSupplierFirstVisit && iteratedValue.isSubSupplierFirstVisit){
                             iteratedValue.isSubSupplierFirstVisit=false;
                             const iteratedAssignment =Object.assign({}, this.props.assignmentSubSupplier[iteratedValue.subSupplierId], iteratedValue);
                             this.props.actions.UpdateSupplierInformation(iteratedAssignment,iteratedValue);
                         }                           
                    });
                }
                this.props.actions.UpdateSubSuppliers(editedData).then(res=>{
                    if(res){
                        this.supplierChild.refreshGrid();
                    }
                });
             /** validate Only one Supplier is first visit ended */
             this.props.actions.AddSubSupplierInformation(editedData,this.editedRowData);
            }
        }
        else{
          
        editedData.recordStatus= "N";

        this.props.actions.UpdateSubSuppliers(editedData).then(res=>{
            if(res){
                this.supplierChild.refreshGrid();
            }
        });
         /** validate Only one Supplier is first visit started */
                if(this.props.subsuppliers){
                    this.props.subsuppliers.forEach(iteratedValue =>{
                         if(editedData.isSubSupplierFirstVisit && iteratedValue.isSubSupplierFirstVisit){
                             iteratedValue.isSubSupplierFirstVisit=false;
                             const assignmentSubSupplierIndex = this.props.assignmentSubSupplier.findIndex(x=>x.subSupplierId === iteratedValue.subSupplierId);
                             const iteratedAssignment =Object.assign({}, this.props.assignmentSubSupplier[assignmentSubSupplierIndex], iteratedValue);
                             this.props.actions.UpdateSupplierInformation(iteratedAssignment,iteratedValue);
                         }                           
                    });
                }
        this.props.actions.AddSubSupplierInformation(editedData,this.editedRowData);      
        }
        this.updatedData = {};
    
    }

    /** Part of Assignment change handler : 
     * get selected row data
     * update part of Assignment in this.updatedData
     * if isPartOfAssignment
     *      Action 1 : update AssignmentSubSupplier in AssignmentDetail (record status as N OR M)
     *      Action 2 : update subSupplier Array used for Grid (without record status)
     * else
     *      if Resource assigned to that supplier
     *          Throw validation popup
     *      else
     *          Action 1 : update AssignmentSubSupplier in AssignmentDetail (record status as D OR splice)
     *          Action 2 : update subSupplier Array used for Grid (without record status)
    */
    
    partOfAssignmentChangeHandler = async (e,data) => {
        const result = formInputChangeHandler(e);
        this.updatedData[result.name] = result.value;
        if(this.updatedData.isPartofAssignment){
            /** Param 1 : Selected Row Data
             *  Param 2 : this.updatedData */
            const response = await this.props.actions.updatePartOfAssignment(data, this.updatedData);
            if(response)
                this.supplierChild.refreshGrid();
        }
        else{
            const assignmentSubSupplier = isEmptyReturnDefault(this.props.assignmentSubSupplier);
            const subSupplierIndex = assignmentSubSupplier.findIndex(x => x.subSupplierId === data.supplierId && x.recordStatus !== 'D');
            const visitSupplier = isEmptyReturnDefault(this.props.visitList);
            const visitSupplierIndex = visitSupplier.findIndex(x => x.visitSupplier === data.subSupplierName);
            if(subSupplierIndex >= 0){
                const subSupplierTS = isEmptyReturnDefault(assignmentSubSupplier[subSupplierIndex].assignmentSubSupplierTS).filter(x => x.recordStatus !== 'D' && x.isAssignedToThisSubSupplier != false && x.isDeleted != true);
                if(subSupplierTS.length > 0){
                    const confirmationObject ={
                        title:modalTitleConstant.CONFIRMATION,
                        message:modalMessageConstant.assignment.supplierInformation.UNASSIGNED_SUB_SUPPLIER,
                        type:'confirm',
                        modalClassName:"warningToast",
                        buttons:[
                            {
                                buttonName:localConstant.commonConstants.OK,
                                onClickHandler:this.confirmationRejectHandler,
                                className:"modal-close m-1 btn-small"
                            }
                        ]
                    };
                    this.props.actions.DisplayModal(confirmationObject);
                    this.supplierChild.refreshGrid();
                } else if(visitSupplierIndex >= 0){
                    const confirmationObject ={
                        title:modalTitleConstant.CONFIRMATION,
                        message:modalMessageConstant.assignment.supplierInformation.CANNOT_REMOVE_PART_OF_ASSIGNMENT,
                        type:'confirm',
                        modalClassName:"warningToast",
                        buttons:[
                            {
                                buttonName:localConstant.commonConstants.OK,
                                onClickHandler:this.confirmationRejectHandler,
                                className:"modal-close m-1 btn-small"
                            }
                        ]
                    };
                    this.props.actions.DisplayModal(confirmationObject);
                    this.supplierChild.refreshGrid();
                } else {
                    /** Param 1 : Selected Row Data
                     *  Param 2 : this.updatedData */
                    const response = await this.props.actions.updatePartOfAssignment(data, this.updatedData);
                    if(response)
                        this.supplierChild.refreshGrid();
                }
            }
        }
        this.updatedData = {};   
    }

    partOfAssignmentHandler = (e,data)=>
    {
        const result = formInputChangeHandler(e);
        this.editedRowData=data;
        this.updatedData[result.name] = result.value;

        if(this.updatedData.isPartofAssignment===true){
            if(this.editedRowData.supplierContactName)
            {
                this.updatedData["supplierContactValidation"]="";
            }
            else {
                this.updatedData["supplierContactValidation"]=localConstant.commonConstants.SUPPLIER_CONTACT_VALIDATION;
            }                
        }
        else{
            this.updatedData["supplierContactValidation"] = "";        
        }       
        const editedData=Object.assign({},this.editedRowData,this.updatedData);
        const assignmentSubSuppliers = Object.assign([],this.props.assignmentSubSupplier);
        const index=assignmentSubSuppliers.findIndex(value => value.subSupplierId === editedData.supplierId);//MS-TS Link CR
        let assignedTechspec =[];
        if(index >= 0){
                assignedTechspec = Object.assign([],assignmentSubSuppliers[index].assignmentSubSupplierTS.filter(y=>y.isAssignedToThisSubSupplier !==false) );//MS-TS Link CR
        }
                                    
        if(this.props.assignmentSubSupplier && this.props.assignmentSubSupplier.length > 0){
            editedData.mainSupplierContactName = this.props.assignmentSubSupplier[0].mainSupplierContactName;
            editedData.mainSupplierContactId = this.props.assignmentSubSupplier[0].mainSupplierContactId;
            editedData.isMainSupplierFirstVisit =this.props.assignmentSubSupplier[0].isMainSupplierFirstVisit;
        }
        assignedTechspec = assignedTechspec.filter(x=>x.recordStatus!="D");//Changes Dec 2
        if(this.updatedData.isPartofAssignment === false && assignedTechspec.length > 0){
            const confirmationObject ={
                title:modalTitleConstant.CONFIRMATION,
                message:modalMessageConstant.assignment.supplierInformation.UNASSIGNED_SUB_SUPPLIER,
                type:'confirm',
                modalClassName:"warningToast",
                buttons:[
                    {
                        buttonName:localConstant.commonConstants.OK,
                        onClickHandler:this.confirmationRejectHandler,
                        className:"modal-close m-1 btn-small"
                    }
                ]
            };
            this.props.actions.DisplayModal(confirmationObject);
        }

        else{
            if(this.props.currentPage === 'editViewAssignment'){
              if  (this.props.assignedtechSpec.length > 0)
                {
                        editedData.isFirstVisitDisabled=true;
                }
                else 
                {
                    if(editedData.isPartofAssignment===true)
                    {          
                        editedData.isFirstVisitDisabled=false;
                    }
                    else{
                        editedData.isSubSupplierFirstVisit=false;
                        editedData.isFirstVisitDisabled=true;
                        editedData.supplierContactId="";
                        editedData.supplierContactName="";
                    }
                } 

                if(editedData.assignmentSubSupplierId){
                    if(editedData.recordStatus !==  'N'){
                        editedData.recordStatus = 'M';
                    }

                    this.props.actions.UpdateSubSuppliers(editedData).then(res=>{
                        if(res){
                            this.supplierChild.refreshGrid();
                        }
                    });
                  
                 /** validate Only one Supplier is first visit ended */
                 this.props.actions.UpdateSupplierInformation(editedData,this.editedRowData);
                }
                else{
                    editedData.recordStatus = 'N';
                    editedData.assignmentId = this.props.assignmentDetails.assignmentId;

                    this.props.actions.UpdateSubSuppliers(editedData).then(res=>{
                        if(res){
                            this.supplierChild.refreshGrid();
                        }
                    });
                 /** validate Only one Supplier is first visit ended */
                 this.props.actions.AddSubSupplierInformation(editedData,this.editedRowData);
                }
            }
            else{
                if(editedData.isPartofAssignment===true)
                { 
                    editedData.isFirstVisitDisabled=false;        
                }
                else{
                    editedData.isFirstVisitDisabled=true;
                    editedData.isSubSupplierFirstVisit=false;
                }
                editedData.recordStatus = "N";
                this.props.actions.UpdateSubSuppliers(editedData).then(res=>{
                    if(res){
                        this.supplierChild.refreshGrid();
                    }
                });
                this.props.actions.AddSubSupplierInformation(editedData,this.editedRowData);  
            }
        }  
        this.updatedData = {};
    }

    confirmationRejectHandler= () =>{  
        this.supplierChild.refreshGrid(); //MS-TS Link CR
        this.props.actions.HideModal();
    }

    cancelSubSupplierModal =(e)=>{
        e.preventDefault();
        this.setState({
            isShowSubSupplierModal: false,
            isShowMainSupplierModal:false
        });
        this.updatedData = {};
        this.editedRowData = {};
    }

    isAssignedtoThisSupplierDisable = () => {
        return this.props.isArsAssignment || this.props.interactionMode; //|| (this.props.isInterCompanyAssignment && this.props.isOperatorCompany); //Changes for D1320
    };

    isSubSupplierDisable = () => {
        return this.props.interactionMode || 
            required(this.props.assignmentDetails.assignmentSupplierPurchaseOrderId) ||
            (this.props.isInterCompanyAssignment && this.props.isOperatorCompany);
    };

    isSubSupplierContactDisable = () => {
        return this.props.interactionMode || 
        required(this.props.assignmentDetails.assignmentSupplierPurchaseOrderId);
    };

    isResourceAssignedToSuppliers = () => {
        const assignmentSubSupplier = this.props.assignmentSubSupplier;
        let isFirstVisitDiable = false;
        assignmentSubSupplier.forEach(x => {
            const techSpec = getNestedObject(x,[ 'assignmentSubSupplierTS' ]);
            if(Array.isArray(techSpec) && techSpec.length > 0)
                isFirstVisitDiable = true;
        });
        return isFirstVisitDiable;
    };

    render() {
        const {
            supplierContacts,
            assignmentDetails,
            subsuppliers,
            subsupplierContacts,
            assignmentSubSupplier,
            assignedtechSpec,
            assignmentTechSpec,
            isInterCompanyAssignment,
            isOperatorCompany
        } = this.props;

        this.ModalButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelSubSupplierModal,
                btnID: "cancelSubsupplierModal",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                btnID: "submitSubsupplierModal",
                btnClass: "waves-effect waves-teal btn-small mr-2",
                showbtn: true
            }
        ];
        this.MainSupplierModalButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelSubSupplierModal,
                btnID: "cancelMainsupplierModal",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                btnID: "submitMainsupplierModal",
                btnClass: "waves-effect waves-teal btn-small mr-2",
                showbtn: true
            }
        ];

        let subsuppliersrow =this.props.subsuppliers;
        //  && this.props.subsuppliers.filter(row=>row.recordStatus !== 'D');
        subsuppliersrow = isEmptyReturnDefault(subsuppliersrow);
        const functionRefs = {};
        functionRefs["disableEditColumn"] = this.disableEditColumn;
        functionRefs["isAssignedtoThisSupplierDisable"] = this.isAssignedtoThisSupplierDisable;
        functionRefs["isSubSupplierDisable"] = this.isSubSupplierDisable;
        functionRefs["isSubSupplierContactDisable"] = this.isSubSupplierContactDisable;
        this.headerData = HeaderData(functionRefs);
        bindAction(this.headerData.subSupplierHeader, "EditSupplierInformation", this.editRowHandler);
        bindAction(this.headerData.subSupplierHeader,"subSupplierContactName",(data,e) => this.subSupplierContactChangeHandler(data,e));
        bindAction(this.headerData.subSupplierHeader,"isPartofAssignment",(data,e) => this.partOfAssignmentChangeHandler(data,e));
        bindAction(this.headerData.subSupplierHeader,"isSubSupplierFirstVisit",(e,data) => this.firstVisitChangeHandler(e,data));
        this.filterdAssignmentSubSupplier = assignmentSubSupplier.filter(x=>x.recordStatus !== "D");//(MSTS - Dec 2)
        
        const isResourceAssigned = this.isResourceAssignedToSuppliers();
        subsuppliersrow.forEach(items=>{
            //MS-TS Link CR Nov 22
            items.isFirstVisitDisabled = isResourceAssigned ? isResourceAssigned : items.isFirstVisitDisabled;
            //MS-TS Link CR Nov 22
        });

        return (          
            <div className="genralDetailContainer customCard">
            <CustomModal />
                <MainSupplier 
                    onChangeHandler={this.onChangeHandler}
                    selectedRowHandler={this.selectedRowHandler}
                    supplierContacts={supplierContacts ? supplierContacts : []}
                    assignmentDetails={assignmentDetails ? assignmentDetails : {}}
                    assignmentSubSupplier={this.filterdAssignmentSubSupplier && this.filterdAssignmentSubSupplier.length > 0 ? this.filterdAssignmentSubSupplier[0] : {}}
                    interactionMode={this.props.interactionMode || required(assignmentDetails.assignmentSupplierPurchaseOrderId)}
                    mainSupplierPopupShowHandler={this.onMainSupplierPopupShowHandler}
                    currentPage={this.props.currentPage}
                    assignedtechSpec={ assignmentTechSpec.filter(x => x.recordStatus !== 'D') }
                    isResourceAssigned = { isResourceAssigned }
                    switchRefProps={this.switchRef}
                     />
                 <CardPanel className="white lighten-4 black-text mb-2" title={localConstant.assignments.SUB_SUPPLIERS} colSize="s12">
                        <ReactGrid gridRowData={this.props.isSubSupplierContactUpdated ? subsuppliersrow : null}
                                gridColData={this.headerData.subSupplierHeader} 
                                rowSelected={this.onSupsupplierRowSelect}
                                onRef={ref=>{this.supplierChild=ref;}}
                                paginationPrefixId={localConstant.paginationPrefixIds.assignmentSubSup}
                            /> 
                        <ReactGrid gridRowData={assignedtechSpec} //d356
                            gridColData={this.headerData.technicalSpecialistHeader}
                            rowSelected={this.onTechSpecRowSelect}
                            rowClassRules={{ allowDangerTag: true }} 
                            rowName="subSupplierTS"
                            onRef={ref => { this.child = ref; }}
                            paginationPrefixId={localConstant.paginationPrefixIds.assignmentTS} />
                    
                </CardPanel>

             {this.state.isShowSubSupplierModal ?
                <Modal id="subsupplierPopup"
                        title='Sub-Supplier'
                        modalClass="projectNoteModal"
                        buttons={this.ModalButtons}
                        onSubmit={this.subSupplierModalSubmitHandler}
                        isShowModal={this.state.isShowSubSupplierModal}>
                        <SubSupplierPopup
                            SubSupplierOnChangeHandler={this.SubSupplierOnChangeHandler}
                            subsupplierContacts={subsupplierContacts}
                            subsupplierName={this.editedRowData.subSupplierName}
                            supplierContactName={this.editedRowData.supplierContactName}
                            subSupplierRowData={this.editedRowData}
                            currentPage={this.props.currentPage}
                            subsupplierContactsRequired={ this.state.subSupplierData.isSubSupplierFirstVisit === true || this.state.subSupplierData.isPartofAssignment === true ? true : false }
                            interactionMode={this.props.interactionMode}
                        />
                </Modal>: null}
            </div>   
        );
    }
}

export default SupplierInformation;
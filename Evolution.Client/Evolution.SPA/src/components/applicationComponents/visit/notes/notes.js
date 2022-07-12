import React, { Component,Fragment } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './notesHeaders';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import { getlocalizeData, formInputChangeHandler,isEmpty, bindAction } from '../../../../utils/commonUtils';
import Modal from '../../../../common/baseComponents/modal';
import dateUtil from '../../../../utils/dateUtil';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import PropTypes from 'prop-types';
import { visitTabDetails } from '../../../viewComponents/visit/visitDetails/visitTabsDetails';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import { fieldLengthConstants } from '../../../../constants/fieldLengthConstants';
import moment from 'moment';

const localConstant = getlocalizeData();

const NotesModal = (props) => {
    return (
        <Fragment>
            <div className="col s12 p-0" >
                <CustomInput
                    hasLabel={true}
                    label={localConstant.visit.VISIT_NOTES}
                    divClassName='col pl-0 mb-4'
                    colSize="col s12"
                    type='textarea'
                    required={true}
                    name="note"
                    id="note"
                    rows="5"
                    inputClass=" customInputs textAreaInView"
                    labelClass="customLabel mandate"
                    maxLength={fieldLengthConstants.common.notes.VISIT_TIMESHEET_MAXLENGTH}
                    defaultValue={props.note ? props.note : ""}
                    onValueChange={props.inputHandleChange}
                    onValueBlur={props.inputHandleChange}
                    autofocus="true"
                    readOnly={props.textReadOnly}
                />
                <CustomInput
                    type='switch'
                    switchLabel={localConstant.visit.CUSTOMER_VISIBLE}
                    isSwitchLabel={true}
                    switchName="visibleToCustomer"
                    id="visibleToCustomer"
                    colSize="s4 pl-0"
                    disabled={props.readOnly}                    
                    checkedStatus={props.visibleToCustomer}
                    onChangeToggle={props.onChangeToggleHandler}
                />
                <CustomInput
                    type='switch'
                    switchLabel={localConstant.visit.RESOURCE_VISIBLE}
                    isSwitchLabel={true}
                    switchName="visibleToSpecialist"
                    id="visibleToSpecialist"
                    colSize="s4 pl-0"
                    disabled={props.readOnly}
                    checkedStatus={props.visibleToSpecialist}
                    onChangeToggle={props.onChangeToggleHandler}
                />                                                                
            </div>
          
        </Fragment>
    );
};

class Notes extends Component {
    constructor(props) {
        super(props);
        this.state = {          
            isNotesShowModal: false,
            notesEdit: false,
            isNoteModelAddView: false
        };
        this.updatedData = {};
         //Add Buttons
         this.notesAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelNotesModal,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.addNotesDetails,
                btnClass: "btn-small ",
                showbtn: true
            }
        ];

        //Edit Buttons
        this.notesEditButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.cancelNotesModal,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type:"button"
            }
        ];
    }
  
   //All Input Handle get Name and Value
   inputHandleChange = (e) => {
        e.preventDefault();
        const inputvalue = formInputChangeHandler(e);
        this.updatedData[inputvalue.name] = inputvalue.value;
    }

    onChangeToggleHandler = (e) => {
        const fieldValue = e.target["checked"];
        const fieldName = e.target.name;
        const result = { value: fieldValue, name: fieldName };
        this.updatedData[result.name] = result.value;        
    }
    
    //Adding Stamp details to the grid
    addNotesDetails = (e) => {
        e.preventDefault();
        const editedData=Object.assign({},this.viewRowData,this.updatedData);
        const date = moment().format(localConstant.commonConstants.SAVE_DATE_FORMAT);
        if (this.updatedData.note === undefined || this.updatedData.note === "") {
            IntertekToaster(localConstant.warningMessages.NOTE_IS_MANDATORY, 'warningToast companyNoteReferenceTypeReq');
        }else if(editedData.visitNoteId || editedData.visitNoteUniqueId){ //D661 issue8
            if(editedData.recordStatus !== "N")
                editedData.recordStatus="M";
            // let date = new Date();
            // date = dateUtil.postDateFormat(date, '-');
            editedData.createdOn =  date;
            editedData.modifiedBy = this.props.loggedInUser;
            this.props.actions.EditVisitNotes(editedData);
            this.cancelNotesModal(e);
        }
        else {
            // let date = new Date();
            // date = dateUtil.postDateFormat(date, '-');
            this.updatedData["recordStatus"] = "N";
            this.updatedData["createdBy"] = this.props.loggedInUser;
            this.updatedData["userDisplayName"] = this.props.loggedInUserName;
            this.updatedData["createdOn"] = date;
            this.updatedData["notes"] = this.updatedData.note;
            this.updatedData["visitNoteUniqueId"] = Math.floor(Math.random() * (Math.pow(10, 5)));  //D661 issue8
            this.props.actions.AddUpdateVisitNotes(this.updatedData);
            this.cancelNotesModal(e);
            //this.hidevisitRefModal();
            this.updatedData = {};    
        }        
    }

    showNotesModal = (e) => {
        e.preventDefault();      
        this.setState((state) => {
            return {
                isNotesShowModal: true,
                notesEdit: false,
                isNoteModelAddView: false
            };
        });        
        this.editedRowData = {};
    }

    //Hiding the modal
    hideNotesModal = (e) => {
        e.preventDefault();
        this.setState((state) => {
            return {
                isNotesShowModal: false,
            };
        });
        this.updatedData = {};
        //this.editedRowData = {};
    }

    //Cancel the updated data in model popup
    cancelNotesModal=(e)=>{
        e.preventDefault();
        this.updatedData = {};  
        this.viewRowData ={};     //D661 issue 8
        this.setState((state) => {
            return {
                isNotesShowModal: false,
            };
        });
    }

    componentDidMount() {
        // if (this.isPageRefresh() && this.props.currentPage === localConstant.visit.EDIT_VIEW_VISIT_MODE) {
        //     this.props.actions.FetchVisitNotes();
        // }
    };

    isPageRefresh() {
        let isRefresh = true;
        visitTabDetails.forEach(row => {
            if(row["tabBody"] === "Notes") {
                isRefresh = row["isRefresh"];
                row["isRefresh"] = false;
                row["isCurrentTab"] = true;
            } else {
                row["isCurrentTab"] = false;
            }
        });
        return isRefresh;
    }

    viewRowHandler = (data) => {
        this.setState({
            isNotesShowModal: true,
            //isNoteModelAddView:false,
            //isbtnDisable: false,
            notesEdit: true, //D661 issue8,
            isNoteModelAddView: true
        }); //D661 issue8
        if(data.createdBy == this.props.loggedInUser){
            this.setState({
                notesEdit:false,
            });
        }

        this.viewRowData = data;
    }

    render() {
        const { interactionMode } = this.props;
       const VisitNoteHeaderData = HeaderData;
       bindAction(VisitNoteHeaderData, "ViewVisitNotes", this.viewRowHandler);
        return (
            <Fragment>
                  {this.state.isNotesShowModal &&
                <Modal
                    modalClass="visitModal"
                    title={localConstant.notes.NOTES}
                    buttons={this.state.notesEdit ? this.notesEditButtons : this.notesAddButtons}
                    isShowModal={this.state.isNotesShowModal}>
                    <NotesModal 
                        inputHandleChange={(e) => this.inputHandleChange(e)}
                        onChangeToggleHandler={(e) => this.onChangeToggleHandler(e)}
                        note = {this.viewRowData && this.viewRowData.note }  //D661 issue8
                        visibleToCustomer = {isEmpty(this.viewRowData)?false:this.viewRowData.visibleToCustomer}
                        visibleToSpecialist = {isEmpty(this.viewRowData)?false:this.viewRowData.visibleToSpecialist}
                        readOnly = {this.state.notesEdit }  //this.props.isOperatorApporved - Removed because of Defect 1426
                        textReadOnly = {this.state.isNoteModelAddView}
                    />
                </Modal>}
                <div className="customCard">
                    <h6 className="bold col s3 pl-0 pr-3">{localConstant.visit.VISIT_NOTES}</h6>
                    <CardPanel className="white lighten-4 black-text" colSize="s12">
                        <ReactGrid 
                            gridRowData={this.props.VisitNotes} 
                            gridColData={VisitNoteHeaderData} 
                            paginationPrefixId={localConstant.paginationPrefixIds.visitNotesId} />
                        {/* this.props.isOperatorApporved - Removed because of Defect 1426 */}
                       {this.props.pageMode !== localConstant.commonConstants.VIEW ? <div className="right-align mt-2">
                            <a className="waves-effect btn-small" disabled={this.props.isTBAVisitStatus || interactionMode} onClick={this.showNotesModal} >{localConstant.commonConstants.ADD}</a>                    
                        </div>:null}
                    </CardPanel>
                </div>
            </Fragment>
            
        );
    }
}

export default Notes;

Notes.propTypes = {
    VisitNotes: PropTypes.array
};

Notes.defaultProps = {
    VisitNotes: []
};
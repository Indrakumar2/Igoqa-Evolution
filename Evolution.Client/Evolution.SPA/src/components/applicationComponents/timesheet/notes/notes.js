import React, { Component, Fragment } from 'react';
import { HeaderData } from './notesHeader.js';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import Modal from '../../../../common/baseComponents/modal';
import { getlocalizeData, isEmpty, bindAction } from '../../../../utils/commonUtils';
import CustomModal from '../../../../common/baseComponents/customModal';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import {
    NoteModalPopup,
    NotesAddButton
} from '../../../commonNotes/notes';
import CustomInput from '../../../../common/baseComponents/inputControlls';
import dateUtil from '../../../../utils/dateUtil';
import PropTypes from 'prop-types';
import moment from 'moment';

const localConstant = getlocalizeData();
const NotesModal = (props) => {
    return (
        <Fragment>
            <div className="col s12 p-0" >
                <CustomInput
                    hasLabel={true}
                    label={localConstant.timesheet.DATE}
                    type='date'
                    name='date'
                    colSize='s4 pl-0'
                    labelClass="mandate"
                    selectedDate={props.date}
                    onDateChange={props.fetchDate}
                    dateFormat={localConstant.commonConstants.CUSTOMINPUT_DATE_FORMAT}
                    placeholderText={localConstant.commonConstants.CUSTOMINPUT_DATE_FORMAT} />

                <CustomInput
                    hasLabel={true}
                    type="text"
                    divClassName='col pl-0'
                    label={localConstant.timesheet.NOTE}
                    colSize='s4 pl-0'
                    inputClass="customInputs"
                    onValueChange={props.onChangeHandler}
                    maxLength={500}
                    name='note'
                />

                <CustomInput
                    hasLabel={true}
                    label={localConstant.timesheet.CUSTOMER_VISIBLE}
                    type='select'
                    name='customerVisible'
                    colSize='s4 pl-0'
                    className="browser-default"
                    optionsList={props.customerVisibleType}
                    optionName='value'
                    optionValue='value'
                    onSelectChange={props.onChangeHandler}
                />
                <CustomInput
                    hasLabel={true}
                    label={localConstant.timesheet.SPECIALIST_VISIBLE}
                    type='select'
                    name='specialist'
                    colSize='s4 pl-0'
                    className="browser-default"
                    optionsList={props.specialistVisibleType}
                    optionName='value'
                    optionValue='value'
                    onSelectChange={props.onChangeHandler}
                />

            </div>

        </Fragment>
    );
};
class Notes extends Component {
    constructor(props) {
        super(props);
        this.updatedData = {};
        this.state = {
            isShowNoteModal: false,
            isNoteModelAddView: false,
            editNotes: false
        };
        this.noteAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.hideModal,
                btnID: "cancelNoteSubmit",
                btnClass: "modal-close waves-effect waves-teal btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.notesSubmitHandler,
                btnID: "noteAddSubmit",
                btnClass: "waves-effect waves-teal btn-small mr-2",
                showbtn: true
            }
        ];
        this.NoteViewButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.hideModal,
                btnID: "cancelNoteSubmit",
                btnClass: "modal-close waves-effect waves-teal  btn-small  mr-2",
                showbtn: true,
                type:"button"
            }
        ];
        this.notesSubmitHandler =this.notesSubmitHandler.bind(this);
    }

    componentDidMount(){
        // if (this.props.currentPage === localConstant.timesheet.EDIT_VIEW_TIMESHEET_MODE)
        //     this.props.actions.FetchTimesheetNotes(this.props.timesheetId);
    }
 
    notesSubmitHandler = (e) => {
        if (!isEmpty(this.updatedData.note)) {
            const editedData=Object.assign({},this.viewRowData,this.updatedData);
            e.preventDefault();
            // let date = new Date();
            // date = dateUtil.postDateFormat(date, '-');
            const date = moment().format(localConstant.commonConstants.SAVE_DATE_FORMAT);
            if(editedData.timesheetNoteId || editedData.timesheetNoteUniqueId){ //D661 issue8
                if(editedData.recordStatus !== "N")
                    editedData.recordStatus="M";
                editedData.createdOn = date;
                editedData.modifiedBy = this.props.loggedInUser;
                this.props.actions.EditTimesheetNotes(editedData);
                this.hideModal(e);
            } else {
            this.updatedData["recordStatus"] = "N";
            this.updatedData["createdBy"] = this.props.loggedInUser;
            this.updatedData["userDisplayName"] = this.props.loggedInUserName;
            this.updatedData["createdOn"] = date;
            this.updatedData["note"] = this.updatedData.note;
            this.updatedData["timesheetId"] = this.props.timesheetId;
            this.updatedData["timesheetNoteUniqueId"] = Math.floor(Math.random() * (Math.pow(10, 5)));  //D661 issue8
            this.props.actions.AddTimesheetNotes(this.updatedData);
            this.updatedData = {};
            this.hideModal(e);
            }
        }
        else
        {
            IntertekToaster(localConstant.validationMessage.TIMESHEET_DETAILS_TIMESHEET_NOTES,"warningToast NotesVal");            
            e.preventDefault();
        }
    }

    inputChangeHandler = (e) => {
        const fieldValue = e.target[e.target.type === "checkbox" ? "checked" : "value"];
        const fieldName = e.target.name;
        const result = { value: fieldValue, name: fieldName };
        return result;
    }
    /* Form Input data Change Handler*/
    formInputChangeHandler = (e) => {
        const result = this.inputChangeHandler(e);
        this.updatedData[result.name] = result.value;
    }
    showNotesModal = (e) => {
        e.preventDefault();
        this.setState((state) => {
            return {
                isNotesShowModal: true,
                notesEdit: false
            };
        });
        this.editedRowData = {};
    }

    uploadNoteHandler = () => {
        this.viewRowData = {};
        this.setState({
            isShowNoteModal: true,
            isNoteModelAddView: true,
            editNotes: false
        });
    }
    noteHangleChange = (e) => {
        const result = this.inputChangeHandler(e);
        this.updatedData[e.target.name] = result;
        this.updatedData["recordStatus"] = "N";
        this.props.actions.AddTimesheetNotes(this.updatedData);
        this.updatedData = {};
    }
    hideModal = (e) => {
        e.preventDefault();
        this.setState({
            isShowNoteModal: false,
            isNoteModelAddView: false,
            editNotes: false
        });
        this.updatedData = {};
        this.viewRowData ={};
    }
    viewRowHandler = (data) => {
        this.setState({
            isShowNoteModal: true,
         //   isNoteModelAddView:false,
            isbtnDisable: false,
            editNotes: true
        }); //D661 issue8
        if(data.createdBy == this.props.loggedInUser){
            this.setState({
                isNoteModelAddView:true,
            });
        }
        this.viewRowData = data;
    }

    render() {
        const { timesheetNotes, interactionMode } = this.props;
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        bindAction(HeaderData, "ViewTimesheetNotes", this.viewRowHandler);
        return (
            <Fragment>
                <CustomModal modalData={modelData} />
                <div className="customCard">
                    <h6 className="bold col s3 pl-0 pr-3">{localConstant.notes.TIMESHEET_NOTES}</h6>
                    <CardPanel className="white lighten-4 black-text" colSize="s12">
                        <ReactGrid gridRowData={timesheetNotes}
                            gridColData={HeaderData}
                            handlerChange={(e)=>this.noteHangleChange(e)}
                            onRef={ref => { this.child = ref; }}
                            paginationPrefixId={localConstant.paginationPrefixIds.timesheetNotesId} />
                       {this.props.pageMode!==localConstant.commonConstants.VIEW? <NotesAddButton uploadNoteHandler={this.uploadNoteHandler}
                            interactionMode={interactionMode}
                        />:null}
                    </CardPanel>
                    {this.state.isShowNoteModal &&
                        <Modal id="contractDocModalPopup"
                            modalClass="projectNoteModal"
                            buttons={this.state.isNoteModelAddView ? this.noteAddButtons : this.NoteViewButtons}
                            isShowModal={this.state.isShowNoteModal}
                            title={localConstant.notes.NOTES}>
                            <NoteModalPopup
                                name={localConstant.notes.TIMESHEET_NOTES}
                                handlerChange={(e) => this.formInputChangeHandler(e)}
                                viewNotes={ this.viewRowData && this.viewRowData.note }                                
                                readOnly={this.state.editNotes}
                                isTimesheetNotes={true}
                            />
                            <CustomInput
                                type='switch'
                                switchLabel={localConstant.timesheet.CUSTOMER_VISIBLE}
                                isSwitchLabel={true}
                                switchName="isCustomerVisible"
                                id="isCustomerVisible"
                                colSize="s4 mb-0"
                                checkedStatus={isEmpty(this.viewRowData)?false: this.viewRowData.isCustomerVisible}
                                onChangeToggle={this.formInputChangeHandler}
                                disabled={this.state.isNoteModelAddView ? this.interactionMode : true}
                            />
                            <CustomInput
                                type='switch'
                                switchLabel={localConstant.timesheet.VISIBLE_TO_TS}
                                isSwitchLabel={true}
                                switchName="isSpecialistVisible"
                                id="isSpecialistVisible"
                                colSize="s4 mb-0"
                                checkedStatus={isEmpty(this.viewRowData)?false: this.viewRowData.isSpecialistVisible}
                                onChangeToggle={this.formInputChangeHandler}
                                disabled={this.state.isNoteModelAddView ? this.interactionMode : true}
                            />
                        </Modal>}
                </div>
            </Fragment>

        );
    };
}

export default Notes;
Notes.propTypes = {
    timesheetNotes: PropTypes.array
};

Notes.defaultProps = {
    timesheetNotes: []
};
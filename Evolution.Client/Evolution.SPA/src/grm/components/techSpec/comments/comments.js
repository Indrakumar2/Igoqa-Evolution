import React, { Component, Fragment } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './headerData.js';
import { getlocalizeData, bindAction, isEmpty, isEmptyReturnDefault,compareObjects } from '../../../../utils/commonUtils';
import CardPanel from '../../../../common/baseComponents/cardPanel';
import Modal from '../../../../common/baseComponents/modal';
import moment from 'moment';
import CustomModal from '../../../../common/baseComponents/customModal';
import {
    NoteModalPopup,
    NotesAddButton
} from '../../../../components/commonNotes/notes';
import { applicationConstants } from '../../../../constants/appConstants';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import { levelSpecificActivities } from '../../../../constants/securityConstant';
import { isEditable,isViewable } from '../../../../common/selector';
const localConstant = getlocalizeData();

class Comments extends Component {
    constructor(props) {
        super(props);
        this.state = {
            date: moment(),
            isCommentShowModal: false,
            commentsViewMode: false,
            isCommentShowGeneralModal:false,
            commentsViewGeneralMode:false,
            isRCRMUpdate:false,
        };
        this.updatedData = {};
        this.userTypes = isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_TYPE));
        bindAction(HeaderData.BusinessInformationHeader, "ViewTSComments", this.viewRowHandler);
        bindAction(HeaderData.ResourceNoteHeader, "ViewTSComments", this.viewGeneralRowHandler);
        this.commentsGeneralAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.commentsGeneralHideModal,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.addGeneralComments,
                btnClass: "btn-small ",
                showbtn: true
            }
        ];
        this.commentsAddButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.commentsHideModal,
                btnClass: "btn-small mr-2",
                showbtn: true,
                type:"button"
            },
            {
                name: localConstant.commonConstants.SUBMIT,
                action: this.addComments,
                btnClass: "btn-small ",
                showbtn: true
            }
        ];
        this.commentsViewGeneralButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.commentsGeneralHideModal,
                btnID: "cancelCreateRateSchedule",
                btnClass: "modal-close waves-effect waves-teal  btn-small  mr-2",
                showbtn: true,
                type:"button"
            }
        ];
        this.commentsViewButtons = [
            {
                name: localConstant.commonConstants.CANCEL,
                action: this.commentsHideModal,
                btnID: "cancelCreateRateSchedule",
                btnClass: "modal-close waves-effect waves-teal  btn-small  mr-2",
                showbtn: true,
                type:"button"
            }
        ];
        this.updateFromRC_RM=[];
    }
    commentsShowModalforGeneral = (e) => {
        e.preventDefault();
        this.setState({
            isCommentShowGeneralModal: true,
            commentsViewGeneralMode: false
        });
        this.viewRowGeneralData = {};
    }
    commentsShowModal = (e) => {
        e.preventDefault();
        this.setState({
            isCommentShowModal: true,
            commentsViewMode: false
        });
        this.viewRowData = {};
    }
    commentsHideModal = (e) => {
        e.preventDefault();
        this.setState({ isCommentShowModal: false });
        this.updatedData = {};
        this.viewRowData = {};
    }
    commentsGeneralHideModal = (e) => {
        e.preventDefault();
        this.setState({ isCommentShowGeneralModal: false });
        this.updatedData = {};
        this.viewRowGeneralData = {};
    }
    formInputHandler = (e) => {
        if((localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RM))|| (localStorage.getItem('UserType').includes(localConstant.techSpec.userTypes.RC))){
            this.setState({ isRCRMUpdate: true });
        } 
        this.updatedData[e.target.name] = e.target.value;
    }
    addComments = (e) => {
        const editedData=Object.assign({},this.viewRowData,this.updatedData);
        e.preventDefault();
        if (this.updatedData && !isEmpty(this.updatedData)) {
            if (this.updatedData.note === undefined || this.updatedData.note === "") {
                IntertekToaster(localConstant.warningMessages.COMMENT_IS_MANDATORY, 'warningToast techSpecCommentReferenceTypeReq');
            }
            else if(editedData.id){ //D661 issue8
                if(editedData.recordStatus !== "N")
                    editedData.recordStatus="M";
                editedData.createdDate = this.state.date.format(localConstant.techSpec.common.DATE_FORMAT);
                this.props.actions.EditComments(editedData);
                this.commentsHideModal(e);
            }
            else{
            this.updatedData["id"] =  - Math.floor(Math.random() * (Math.pow(10, 5))); //sanity 200 fix
            this.updatedData["epin"] = this.props.epin!==undefined ? this.props.epin : 0;
            this.updatedData["recordStatus"] = "N";
            this.updatedData["recordType"] = "TSComment";
            this.updatedData["createdBy"] = this.props.loginUser;
            this.updatedData["ePin"]= this.props.epin!==undefined ? this.props.epin : 0;
            this.updatedData["createdDate"] = this.state.date.format(localConstant.techSpec.common.DATE_FORMAT);
            this.props.actions.AddComments(this.updatedData);
            this.setState((state) => {
                return {
                    isCommentShowModal:false
                };
            });
            this.updatedData = {};
            this.viewRowData = {};
        }
        }
    }

    addGeneralComments = (e) => {
        const editedData=Object.assign({},this.viewRowGeneralData,this.updatedData);
        e.preventDefault();
        if (this.updatedData && !isEmpty(this.updatedData)) {
            if (this.updatedData.note === undefined || this.updatedData.note === "") {
                IntertekToaster(localConstant.warningMessages.COMMENT_IS_MANDATORY, 'warningToast techSpecCommentReferenceTypeReq');
            }
            else if(editedData.id){ //D661 issue8
                if(editedData.recordStatus !== "N")
                    editedData.recordStatus="M";
                editedData.createdDate = this.state.date.format(localConstant.techSpec.common.DATE_FORMAT);
                this.props.actions.EditComments(editedData);
                this.commentsGeneralHideModal(e);
            }
            else{
                this.updatedData["id"] = - Math.floor(Math.random() * (Math.pow(10, 5))); //sanity 200 fix
            this.updatedData["epin"] = this.props.epin!==undefined ? this.props.epin : 0;
            this.updatedData["recordStatus"] = "N";
            this.updatedData["recordType"] = "General";
            this.updatedData["createdBy"] = this.props.loginUser;
            this.updatedData["ePin"]= this.props.epin!==undefined ? this.props.epin : 0;
            this.updatedData["createdDate"] = this.state.date.format(localConstant.techSpec.common.DATE_FORMAT);
            this.updateFromRC_RM.push(this.updatedData);
            this.props.actions.AddComments(this.updatedData);
            this.setState((state) => {
                return {
                    isCommentShowGeneralModal:false
                };
            });
            this.updatedData = {};
            this.viewRowGeneralData = {};
        }
        }
    }
    viewRowHandler = (data) => {
        this.setState((state) => {
            return {
                isCommentShowModal: !state.isCommentShowModal,
                commentsViewMode: true
            };
        });
        //Commented because currently on hold
        // if(data.createdBy == this.props.loginUser){
        //     this.setState({
        //         commentsViewMode:false,
        //     });
        // }
        this.viewRowData = data;
    }
    viewGeneralRowHandler = (data) => {
        this.setState((state) => {
            return {
                isCommentShowGeneralModal: !state.isCommentShowGeneralModal,
                commentsViewGeneralMode: true
            };
        });
        //Commented because currently on hold
        // if(data.createdBy == this.props.loginUser){
        //     this.setState({
        //         commentsViewGeneralMode:false,
        //     });
        // }
        this.viewRowGeneralData = data;
    }
    /** Field Disable Handler */
    fieldDisableHandler = () => {
        if(this.props.isTSUserTypeCheck){
            return false;
        }
        if (this.props.techManager) {
            return !isEditable({ activities: this.props.activities, editActivityLevels: levelSpecificActivities.editActivitiesLevel0 });
        }
        else  if (this.props.isTMUserTypeCheck && !isViewable({ activities: this.props.activities, levelType: 'LEVEL_3',viewActivityLevels: levelSpecificActivities.viewActivitiesTM }) && !this.props.isRCUserTypeCheck) {
            return false;
        } //D1374
        return false;
    };

    fieldvisibleHandler = () =>
    {
        if(this.userTypes.includes(localConstant.techSpec.userTypes.TS))
        {
            return false;
        }
        return true;

    }
    customerApprovedDraftData = (originalData) => {
        let work = 0;
        if (originalData && originalData.data ) {
            if (!isEmpty(this.props.draftDataToCompare) && ( this.props.isRCUserTypeCheck || this.props.isRMUserTypeCheck )) {
                work = this.props.draftDataNotes.filter(x => x.id === originalData.data.id);
                if (work.length > 0) {
                    const result = compareObjects(this.excludeProperty(work[0]), this.excludeProperty(originalData.data));
                    if (!result) {
                        return false;   //RCRM Editing exgisting Row not showing highlight 
                    }
                    return !result;
                } else if (!isEmpty(this.updateFromRC_RM)) {
                    work = this.updateFromRC_RM.filter(x => x.id === originalData.data.id);
                    if (work.length > 0) {
                        const currentResult = compareObjects(work[0], originalData.data);
                        return !currentResult;
                    }
                    else if (this.state.isRCRMUpdate) {
                        work = this.props.selectedDraftNotes.filter(x => x.id === originalData.data.id);
                        if (work.length > 0) {
                            const compareDataResult = compareObjects(this.excludeProperty(work[0]), this.excludeProperty(originalData.data));
                            return compareDataResult;
                        }
                    }
                }
                else if (!isEmpty(this.props.selectedDraftNotes)) {
                    work = this.props.selectedDraftNotes.filter(x => x.id === originalData.data.id);
                    if (work.length > 0) {
                        const compareDataResult = compareObjects(this.excludeProperty(work[0]), this.excludeProperty(originalData.data));
                        return compareDataResult;
                    }
                } else {
                    return false;
                }
            } else {
                this.updateFromRC_RM = [];
                return false;
            }
        }
        return false;
    }

    excludeProperty = (object) => {
        delete object.recordStatus;
        delete object.lastModification;
        delete object.updateCount;
        delete object.modifiedBy;
        return object;
    }
    render() {
        const { commentsData } = this.props;
        const modelData = { ...this.confirmationModalData, isOpen: this.state.isOpen };
        const disableField = this.fieldDisableHandler();
        const visiblefield= this.fieldvisibleHandler();
        const username = isEmptyReturnDefault(localStorage.getItem(applicationConstants.Authentication.USER_NAME));
        const isRCProfile = (this.props.selectedProfileDetails.profileAction === localConstant.techSpec.common.SEND_TO_RC_RM) && (this.props.assignedToUser !== username);
        return (
            <Fragment>
                <CustomModal modalData={modelData} />
                {this.state.isCommentShowModal &&
                    <Modal modalClass="contractModal" 
                        buttons={this.state.commentsViewMode && this.state.commentsViewMode ? this.commentsViewButtons : this.commentsAddButtons}
                        isShowModal={this.state.isCommentShowModal} >
                        <NoteModalPopup
                        name={this.state.commentsViewMode?localConstant.techSpec.comments.VIEW_BUSINESS_INFORMATION_COMMENTS:localConstant.techSpec.comments.ADD_BUSINESS_INFORMATION_COMMENTS}
                        handlerChange={this.formInputHandler}
                        viewNotes={this.viewRowData.note}
                        readOnly={this.state.commentsViewMode?true:false}
                        /> 
                    </Modal>
                }
                  {this.state.isCommentShowGeneralModal &&
                    <Modal modalClass="contractModal" 
                        buttons={this.state.commentsViewGeneralMode && this.state.commentsViewGeneralMode ? this.commentsViewGeneralButtons : this.commentsGeneralAddButtons}
                        isShowModal={this.state.isCommentShowGeneralModal} >
                        <NoteModalPopup
                        name={this.state.commentsViewGeneralMode?localConstant.techSpec.comments.VIEW_BUSINESS_INFORMATION_NOTES:localConstant.techSpec.comments.ADD_BUSINESS_INFORMATION_NOTES}
                        handlerChange={this.formInputHandler}
                        viewNotes={this.viewRowGeneralData.note}
                        readOnly={this.state.commentsViewGeneralMode?true:false}
                        /> 
                    </Modal>
                }
                <div className="genralDetailContainer customCard">
                   
                { visiblefield ?  <CardPanel className="white lighten-4 black-text" title={localConstant.techSpec.comments.BUSINESS_INFORMATION_COMMENTS} colSize="s12" >
                           { visiblefield ? <ReactGrid gridColData={HeaderData.BusinessInformationHeader} gridRowData={commentsData && commentsData.filter(x => x.recordStatus !== "D") && commentsData.filter(x => x.recordType !== "General")}
                            onRef={ref => { this.child = ref; }} paginationPrefixId={localConstant.paginationPrefixIds.techSpecBusinessNotesId} /> : null }

                           { visiblefield ?this.props.pageMode !== localConstant.commonConstants.VIEW &&
                              <NotesAddButton uploadNoteHandler={this.commentsShowModal}  
                              interactionMode={this.props.interactionMode || disableField || isRCProfile}/>  :null 
                           }
                   </CardPanel> :null }
                  
                   <CardPanel className="white lighten-4 black-text" title={localConstant.techSpec.comments.GENERAL_INFORMATION_COMMENTS} colSize="s12" >
                         <ReactGrid gridColData={HeaderData.ResourceNoteHeader} gridRowData={commentsData && commentsData.filter(x => x.recordStatus !== "D") && commentsData.filter(x => x.recordType === "General")}
                            onRef={ref => { this.secondChild = ref; }} rowClassRules={{ rowHighlight: true }} draftData={this.customerApprovedDraftData} paginationPrefixId={localConstant.paginationPrefixIds.techSpecNotesId} />                        
                          { this.props.pageMode !== localConstant.commonConstants.VIEW &&
                            <NotesAddButton uploadNoteHandler={this.commentsShowModalforGeneral}
                                interactionMode={this.props.interactionMode || disableField || isRCProfile} />
                          }
                    </CardPanel>
                </div>
            </Fragment>
        );
    }
};
export default Comments;
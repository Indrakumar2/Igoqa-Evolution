import React, { Component, Fragment } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { headerData } from './viewUserHeader';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import { getlocalizeData, bindAction,ObjectIntoQuerySting } from '../../../../utils/commonUtils';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import CustomModal from '../../../../common/baseComponents/customModal';

const localConstant = getlocalizeData();

class ViewUser extends Component {
    constructor(props) {
        super(props);

        this.state = { isRedirectingToUserPage: false };
    }

    componentDidMount() {
        this.props.actions.SearchUser();
        this.props.actions.SetCurrentPageMode();
    }

    componentWillUnmount() {
        this.props.actions.ResetUserLandingPageDataState();
        // if (!this.state.isRedirectingToUserPage)
        //     this.props.actions.ResetUserDetailState();
        this.props.actions.ResetRolesState();
        this.props.actions.ResetCompanyOfficeState();
    }

    onCreateUser = () => {
        this.props.actions.ResetUserDetailState();
        this.props.history.push('/CreateUser');
    }

    editRowHandler = (data) => {     
        const queryObj={            
            logonName:data.logonName,           
        };
        const queryStr = ObjectIntoQuerySting(queryObj);
        const queryparam=decodeURIComponent(queryStr);  // D-732 Fix
        // this.props.actions.SetUserDetailData(data.logonName)
        //     .then(res => {          
        //         if (res == true) {
        //             setTimeout(x => {
        //                 this.setState({ isRedirectingToUserPage: true });
        //                 //this.props.history.push('/UpdateUser');
        //                 window.open('/UpdateUser?'+queryparam, '_blank');    // D-732 Fix                   
        //             }, 1000);
        //         }
        //     });
        window.open('/UpdateUser?'+queryparam, '_blank'); 
    }

    //Showing modal popup for delete
    deleteUserHandler = () => {
        const selectedRecords = this.gridChild.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.USER_DELETE_MESSAGE,
                type: "confirm",
                modalClassName: "warningToast",
                buttons: [
                    {
                        buttonName: localConstant.commonConstants.YES,
                        onClickHandler: this.deleteSelected,
                        className: "modal-close m-1 btn-small"
                    },
                    {
                        buttonName: localConstant.commonConstants.NO,
                        onClickHandler: this.confirmationRejectHandler,
                        className: "modal-close m-1 btn-small"
                    }
                ]
            };
            this.props.actions.DisplayModal(confirmationObject);
        }
        else
            IntertekToaster(localConstant.techSpec.common.REQUIRED_DELETE, 'warningToast');
    }

    //Deleting the grid detail
    deleteSelected = () => {
        const selectedData = this.gridChild.getSelectedRows();
        const logonNames = [];
        selectedData.map(x => {
            logonNames.push(x.logonName);
        });

        this.props.actions.DeleteUser(logonNames)
            .then(res => {
                if (res == true) {
                    this.gridChild.removeSelectedRows(selectedData);
                }
            });
        this.props.actions.HideModal();
    }

    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }

    render() {
        bindAction(headerData.userHeader, "EditColumn", this.editRowHandler);
        const { userLandingPageData ,pageMode,activities } = this.props;
        // const  delRole=activities.filter(x=>x.activity==="D00001");
        const  newRole=activities.filter(x=>x.activity==="N00001");
        return (
            <Fragment>
                <CustomModal />
                <div className="customCard">  
                <h6 className="pl-2"> <span className="bold">Users :</span> Create/Edit/View Users</h6>              
                        <ReactGrid gridRowData={userLandingPageData} gridColData={headerData.userHeader} onRef={ref => { this.gridChild = ref; }} />
                  
                    {pageMode===localConstant.commonConstants.VIEW?null: <div className="right-align mt-2">
                        {newRole.length>0?<a className="waves-effect btn-small" onClick={this.onCreateUser} >{localConstant.commonConstants.ADD}</a>:null}
                       {/* {delRole.length>0?<a className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn" onClick={this.deleteUserHandler}
                            disabled={userLandingPageData && userLandingPageData.filter(x => x.recordStatus !== "D").length <= 0 ? true : false} >
                            {localConstant.commonConstants.DELETE}</a>:null} */}
                    </div>}
                </div>
            </Fragment>
        );
    }
}

export default ViewUser;

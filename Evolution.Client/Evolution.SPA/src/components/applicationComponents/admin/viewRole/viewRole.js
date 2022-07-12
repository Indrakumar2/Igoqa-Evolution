import React, { Component, Fragment } from 'react';
import ReactGrid from '../../../../common/baseComponents/reactAgGrid';
import { HeaderData } from './viewRoleHeader';
//import Panel from '../../../../common/baseComponents/panel';
//import CustomInput from '../../../../common/baseComponents/inputControlls';
import { modalMessageConstant, modalTitleConstant } from '../../../../constants/modalConstants';
import { getlocalizeData, bindAction,ObjectIntoQuerySting,parseQueryParam } from '../../../../utils/commonUtils';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
import CustomModal from '../../../../common/baseComponents/customModal';
const localConstant = getlocalizeData();

class ViewRole extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isRoleEdit: false
        };
        this.updatedData = {};
        this.editedRowData = {};
    }
    componentDidMount() {
        this.props.actions.FetchViewRole(); 
        this.props.actions.SetCurrentPageMode();
    }
    onCreateRole = () => {
        window.open('/CreateRole', '_blank');
        //this.props.history.push('/CreateRole');        
        this.props.actions.selectedRowData({});
    }
    editRowHandler = (data) => {
           
        const queryObj={            
            description: (data.description === null) ? "" : data.description,
            eventId:data.eventId,           
            roleId:data.roleId,
            roleName:data.roleName,          
            userCompanyCode:data.userCompanyCode,
            isAllowedDuringLock:data.isAllowedDuringLock
        };
        const queryStr = ObjectIntoQuerySting(queryObj);       
        window.open('/UpdateRole?'+ queryStr, '_blank');       
        //this.props.history.push('/UpdateRole');
        this.editedRowData = data;
        // this.setState((state) => {
        //     return {
        //         isRoleEdit: true                
        //     };
        // });      
        this.props.actions.selectedRowData(data);
    }
    //Showing modal popup for delete
    deleteRoleHandler = () => {
        const selectedRecords = this.gridChild.getSelectedRows();
        if (selectedRecords.length > 0) {
            const confirmationObject = {
                title: modalTitleConstant.CONFIRMATION,
                message: modalMessageConstant.ROLE_DELETE_MESSAGE,
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
        else IntertekToaster(localConstant.techSpec.common.REQUIRED_DELETE, 'warningToast');
    }
    //Deleting the grid detail
    deleteSelected = () => {
        const selectedData = this.gridChild.getSelectedRows();        
        this.props.actions.DeleteRoleDetails(selectedData)
            .then(response => {
                if (response && response.code === "1") {                    
                    this.gridChild.removeSelectedRows(selectedData);                    
                }
            });
        
        this.props.actions.HideModal();
    }
    confirmationRejectHandler = () => {
        this.props.actions.HideModal();
    }
    render() {
        bindAction(HeaderData, "EditColumn", this.editRowHandler);
        const { roleData,pageMode,activities } = this.props;
        // const  delRole=activities.filter(x=>x.activity==="D00001");
        const  newRole=activities.filter(x=>x.activity==="N00001");
        return (
            <Fragment>
                <CustomModal />
                <div className="customCard">
                <h6 className="pl-2"> <span className="bold">User Roles :</span> Create/Edit/View User Roles</h6>
                 <ReactGrid gridRowData={roleData} gridColData={HeaderData} onRef={ref => { this.gridChild = ref; }} />
              
                   {pageMode===localConstant.commonConstants.VIEW?null: <div className="right-align mt-2">
                       {newRole.length>0?<a target='_blank' className="waves-effect btn-small" onClick={this.onCreateRole} >{localConstant.commonConstants.ADD}</a>:null}
                       {/* {delRole.length>0?<a className="btn-small ml-2 modal-trigger waves-effect btn-primary dangerBtn" onClick={this.deleteRoleHandler}
                            disabled={roleData && roleData.filter(x => x.recordStatus !== "D").length <= 0 ? true : false} >
                            {localConstant.commonConstants.DELETE}</a>:null} */}
                    </div>}
                    </div>               
            </Fragment>
        );
    }
}

export default ViewRole;

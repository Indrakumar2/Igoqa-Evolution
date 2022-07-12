import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { AppDashBoardRoutes } from '../../../../routes/routesConfig';
import { getlocalizeData,ObjectIntoQuerySting } from '../../../../utils/commonUtils';
import { securitymodule ,contractActivityCode } from '../../../../constants/securityConstant';
import IntertekToaster from '../../../../common/baseComponents/intertekToaster';
const localConstant = getlocalizeData();
class ContractAnchor extends Component {   
    // selectedRowHandler = (event) => {       
    //     if(this.props.location.pathname === AppDashBoardRoutes.assignments || this.props.location.pathname === AppDashBoardRoutes.inactiveassignments){
    //         const contractNumber = { contractNumber:this.props.data.assignmentContractNumber };
    //         this.props.actions.GetSelectedContractData(contractNumber);
    //         this.props.actions.FetchContractForDashboard(contractNumber);
    //     }
    //     else if(this.props.location.pathname === AppDashBoardRoutes.ContractsNearExpiry || this.props.location.pathname === "/Company/Contracts"|| this.props.location.pathname === "/Customer/Contracts"){
    //         const contractNumber = { contractNumber:this.props.data.contractNumber };
    //         this.props.actions.GetSelectedContractData(contractNumber);
    //         this.props.actions.FetchContractForDashboard(contractNumber);
    //     }
    //     else{         
    //         const contractNumber = { contractNumber:this.props.data.contractNumber };
    //         this.props.actions.GetSelectedContractData(contractNumber);
    //         //this.props.actions.GetSelectedCustomerName(this.props.data);
    //     }      
        
    //     // this.props.actions.UpdateCurrentPage("Edit/View Contract");
    //     // this.props.actions.UpdateCurrentModule(localConstant.moduleName.CONTRACT);
    // }
    //D-1351 Fix
    openContract = (redirectionURL,queryStr) => {
        window.open(redirectionURL +`?`+queryStr,'_blank');
    }
    selectedRowHandler = (e,redirectionURL,queryStr) => {      
        const { data } =this.props;
        let contractCompanyCode = data.contractHoldingCompanyCode;
        this.props.actions.FetchUserPermission(this.props.logonUser,this.props.selectedCompany,securitymodule.CONTRACT)
        .then(res => { 
            const contract_global_view =res.result.filter(x=> x.activity == contractActivityCode.View_Global);
            if(this.props.currentPage === localConstant.sideMenu.DASHBOARD){
                if(data.contractHoldingCompanyCode == undefined && data.assignmentContractHoldingCompanyCode != undefined){
                    contractCompanyCode =data.assignmentContractHoldingCompanyCode;
                }
            }
            if (contractCompanyCode != "" && contractCompanyCode === this.props.selectedCompany || (contract_global_view !=undefined && contract_global_view.length >0 )) {
                this.openContract(redirectionURL, queryStr);
            }
            else {
                IntertekToaster("Selected Contract is owned by another Company.", 'warningToast');
            }
        });
    }
    //D-1351 Fix
    render() {
        let contractNumber = "";       
        if( this.props.location.pathname === AppDashBoardRoutes.assignments || this.props.location.pathname === AppDashBoardRoutes.inactiveassignments){
            contractNumber = this.props.data.assignmentContractNumber;
        }
        else if(this.props.location.pathname === AppDashBoardRoutes.ContractsNearExpiry || this.props.location.pathname === "/Company/Contracts"|| this.props.location.pathname === "/Customer/Contracts"){
            contractNumber = this.props.data.contractNumber;
        }
        else{
            contractNumber = this.props.data.contractNumber;
        }
        let redirectionURL = "/ContractsDetails";

        /*Start -- Code added for project module*/
        if (this.props.location.pathname === '/Project/SearchContract') {
            redirectionURL = "/CreateProject";
        }
        /*End -- Code added for project module*/
        const queryObj={
            contractNumber:contractNumber,
            selectedCompany:this.props.selectedCompany
        };
        const queryStr = ObjectIntoQuerySting(queryObj);

        return (
            // <Link target='_blank' to={{ pathname:redirectionURL, search:`?`+queryStr }}  className="link"> {contractNumber}</Link>
            // <a onClick={this.selectedRowHandler} className="link">{contractNumber}</a>
            <a  href='javascript:void(0)' onClick={(e) => this.selectedRowHandler(e,redirectionURL,queryStr)} className="link">{contractNumber}</a>  //D-1351
        );
    }
}

export default ContractAnchor;
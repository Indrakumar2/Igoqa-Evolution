import SelectDocumentValue from './selectDocumentType';
import { bindActionCreators } from 'redux';
import { connect } from "react-redux";
import { isEmptyReturnDefault, isUndefined } from '../../utils/commonUtils';
import { filterDocTypes } from '../../common/selector';
import { UpdateDocumentDetails as CompanyDocument } from '../../components/viewComponents/company/companyAction';
import { UpdateDocumentDetails as CustomerDocument } from '../../components/viewComponents/customer/customerAction';
import { UpdateDocumentDetails as ContractDocument } from '../../actions/contracts/documentAction';
import { UpdateProjectDocumentDetails as ProjectDocument } from '../../actions/project/documentAction';
import { UpdateSupplierDocumentDetails as SupplierDocument } from '../../actions/supplier/supplierDocumentAction';
import { UpdateSupplierPODocumentDetails as SupplierPODocument } from '../../actions/supplierPO/supplierPODocumentAction';
import { UpdateAssignmentDocumentDetails as AssignmentDocument } from '../../actions/assignment/assignmentDocumentAction';
import { UpdateVisitDocumentDetails as VisitDocument } from '../../actions/visit/documentsAction';
import { UpdateTimesheetDocumentDetails as TimesheetDocument } from '../../actions/timesheet/timesheetDocumentAction';
import { UpdateTechSpecDocDetails as TechSpecDocument } from '../../grm/actions/techSpec/techSpecDocumentAction';

const mapStateToProps = (state) => {
    return {       
        // CompanyDocument: state.CompanyReducer.masterDocumentTypeData,
        CompanyDocument:filterDocTypes({ docTypes:state.masterDataReducer.documentTypeMasterData, moduleName:'Company' }),
        // CustomerDocument: state.CustomerReducer.masterDocumentTypeData,
        CustomerDocument:filterDocTypes({ docTypes:state.masterDataReducer.documentTypeMasterData, moduleName:'Customer' }),
        ContractDocument: filterDocTypes({ docTypes:state.masterDataReducer.documentTypeMasterData,moduleName:'Contract' }),
        ProjectDocument: filterDocTypes({ docTypes:state.masterDataReducer.documentTypeMasterData,moduleName:'Project' }),
        // SupplierDocument: state.RootSupplierReducer.SupplierDetailReducers.documentsTypeData,
        SupplierDocument:filterDocTypes({ docTypes:state.masterDataReducer.documentTypeMasterData, moduleName:'Supplier' }),
        //SupplierPODocument:state.rootSupplierPOReducer.documentsTypeData,
        SupplierPODocument:filterDocTypes({ docTypes:state.masterDataReducer.documentTypeMasterData, moduleName:'Supplier PO' }),
        AssignmentDocument:filterDocTypes({ docTypes: state.masterDataReducer.documentTypeMasterData, moduleName:'Assignment' }),
        VisitDocument: filterDocTypes({ docTypes:state.masterDataReducer.documentTypeMasterData, moduleName:'Visit' }),
        TimesheetDocument:filterDocTypes({ docTypes:state.masterDataReducer.documentTypeMasterData, moduleName:'Visit' }),
        TechSpecDocument:filterDocTypes({ docTypes:state.masterDataReducer.documentTypeMasterData, moduleName:'Technical Specialist' }),   
    };    
};

const mapDispatchToProps = dispatch => {
    return {
        actions: bindActionCreators(
            { 
                CompanyDocument,
                CustomerDocument,
                ContractDocument,
                ProjectDocument,
                SupplierDocument,
                SupplierPODocument,
                AssignmentDocument,
                VisitDocument,
                TimesheetDocument,
                TechSpecDocument

            },
            dispatch
        ),
    };
};

export default connect(mapStateToProps, mapDispatchToProps)(SelectDocumentValue);
import React, { Fragment } from 'react';
import { getlocalizeData,fetchCoordinatorDetails,isEmpty } from '../../../utils/commonUtils';
import CardPanel from '../../../common/baseComponents/cardPanel';
import { CustomerContactInfoDiv, MaterialInfoDiv, SupplierInfoDiv, AssignMentType, CategoryInfoDiv, Btn, CompanyCoordinater, FirstVisit,CustomerApproval } from '../resourceFields';
import SubSupplierDetails from '../subSupplierDetails';
import arrayUtil from '../../../utils/arrayUtil';
const localConstant = getlocalizeData();

const assignmentType = (assignmentTypeList,typeCode) => {
  const assignmentTypeName = [];
  assignmentTypeList && assignmentTypeList.forEach(iteratedValue => {
    if(iteratedValue.description === typeCode){
      assignmentTypeName.push(iteratedValue.name);
    }
  });
  return assignmentTypeName;
};

export const MoreDetails = (props) => {
  const { currentPage, isPreAssignmentPage, inputChangeHandler, taxonomyCategory, taxonomySubCategory, taxonomyServices,
    assignmentTypes, isARSSearch, isQuickSearch,gridData,gridHeaderData,subSupplierGridBtn,gridRef,moreDetailsData = {},
    companyList,contractHoldingCoodinatorList,operatingCoordinatorList,firstVisitFrom,firstVisitTo,isDashboardARSView,
    assignMentStatusList, chCoordinatorInfo, opCoordinatorInfo } = props;
  const sortedtaxonomyCategory=arrayUtil.sort(taxonomyCategory, 'name', 'asc');
  const chCoordinatorUsername = chCoordinatorInfo && chCoordinatorInfo.userName;
  const opCoordinatorUsername = opCoordinatorInfo && opCoordinatorInfo.userName;
  const assignmentTypeARS = assignmentType(assignmentTypes,moreDetailsData.assignmentType);
  let filteredAssignmentType = assignmentTypes;
  if(isPreAssignmentPage && !isEmpty(assignmentTypes)){
    const tabsToHide = [ 'A','R' ];
    filteredAssignmentType = arrayUtil.negateFilter(assignmentTypes, 'description', tabsToHide);
  }

  return (
    <Fragment>
      {!isARSSearch &&
        <div className="row mb-0 ml-0 mr-0">
          <CustomerContactInfoDiv
            inputChangeHandler={inputChangeHandler}
            isQuickSearch={isQuickSearch}
            isPreAssignmentPage={isPreAssignmentPage}
            moreDetailsData={moreDetailsData}
            interactionMode={props.interactionMode} />
          {/* <MaterialInfoDiv
            inputChangeHandler={inputChangeHandler}
            isQuickSearchPage={isQuickSearch}
            moreDetailsData={moreDetailsData} 
            isPreAssignmentPage={isPreAssignmentPage} /> */}
          <SupplierInfoDiv
            currentPage={currentPage}
            isPreAssignmentPage={isPreAssignmentPage}
            isARSSearch={isARSSearch}
            isQuickSearchPage={isQuickSearch}
            inputChangeHandler={inputChangeHandler}
            moreDetailsData={moreDetailsData} 
            isQuickSearch={isQuickSearch}
            interactionMode={props.interactionMode}/>
          {props.isSubSupplierGrid &&
            <CardPanel title={localConstant.assignments.SUB_SUPPLIERS} className="white lighten-4 black-text left full-width mb-3" colSize="s12 subSupplierGrid">
              <SubSupplierDetails
                gridData={gridData}
                gridRef={gridRef}
                gridHeaderData={gridHeaderData}
                buttons={subSupplierGridBtn}
                interactionMode={props.interactionMode} />
            </CardPanel>
          }
          <AssignMentType
            inputChangeHandler={inputChangeHandler}
            isQuickSearch={isQuickSearch}
            isPreAssignmentPage={isPreAssignmentPage}
            assignmentType={filteredAssignmentType}
            assignMentStatusList = {assignMentStatusList}
            assignmentTypeData = { !isEmpty(assignmentTypeARS) && assignmentTypeARS[0] }
            moreDetailsData={moreDetailsData}
            interactionMode={props.interactionMode}
             />

          <FirstVisit
            isQuickSearch={isQuickSearch}
            inputChangeHandler={inputChangeHandler}
            isPreAssignment={isPreAssignmentPage}
            firstVisitFrom={firstVisitFrom}
            firstVisitTo={firstVisitTo}
            moreDetailsData={moreDetailsData}
            firstVisitChange={props.firstVisitChange}
            interactionMode={props.interactionMode} />

          <CategoryInfoDiv
            inputChangeHandler={inputChangeHandler}
            taxonomyCategory={sortedtaxonomyCategory}
            taxonomySubCategory={taxonomySubCategory}
            taxonomyServices={taxonomyServices}
            moreDetailsData={moreDetailsData} 
            isQuickSearch={isQuickSearch}
            interactionMode={props.interactionMode}/>
{/* 
          <CustomerApproval 
            defaultCustomerApprovalMultiSelection={props.defaultCustomerApprovalMultiSelection}
            taxonomyCustomerApproved={props.taxonomyCustomerApproved}
            customerApprovalMultiselectValueChange={props.customerApprovalMultiselectValueChange}
          /> */}

          <Btn buttons={props.buttons} onClick={props.onClick} interactionMode={props.interactionMode}/>
        </div>
      }
      {isARSSearch && <Fragment>
        <div className="row mb-0">
          <CompanyCoordinater 
            interactionMode={props.interactionMode} 
            moreDetailsData = {moreDetailsData}
            companyList = { companyList }
            contractHoldingCoodinatorList = { contractHoldingCoodinatorList }
            operatingCoordinatorList = { operatingCoordinatorList }
            opCoordinator = { opCoordinatorUsername }
            chCoordinator = { chCoordinatorUsername }
            isDashboardARSView={isDashboardARSView}
             />
        </div>
      </Fragment>}
    </Fragment>
  );
};
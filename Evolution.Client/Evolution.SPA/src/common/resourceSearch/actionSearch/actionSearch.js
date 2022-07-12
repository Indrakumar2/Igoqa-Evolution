import React from 'react';
import { getlocalizeData } from '../../../utils/commonUtils';
import { Action,Btn } from '../resourceFields';

export const ActionSearch = (props) => {
  const { actionList,currentPage,inputChangeHandler,isPreAssignmentPage,multiSelectOptionsList,defaultMultiSelection,optionAttributs,multiSelectValueChange,
    actionData,isARSSearch,dispositionTypeList,isQuickSearch,buttons,operationalManagers,technicalSpecialists,
    techSpecMultiSelectChangeHandler,defaultTechSpecMultiSelection,overrideGridRowData,overrideGridHeaderData,
    selectedMyTask,isDashboardARSView,hasRejectedResources,rejectedResourceValue,overrideTaskStatus,
    commentsHistoryClickHandler } = props;
  return (
    <div className="row mb-0">
      <Action
        actionList={ actionList }
        dispositionTypeList={dispositionTypeList}
        actionData = {actionData}
        inputChangeHandler={inputChangeHandler}
        onValueChange={multiSelectValueChange}
        multiSelectOptionsList={multiSelectOptionsList}
        defaultMultiSelection={defaultMultiSelection}
        optionAttributs={optionAttributs}
        isPreAssignmentPage = { isPreAssignmentPage }
        isARSSearch = {isARSSearch}
        isQuickSearch = {isQuickSearch} 
        operationalManagers = {operationalManagers}
        technicalSpecialists = {technicalSpecialists}
        defaultActionValue={props.defaultActionValue}
        techSpecMultiSelectChangeHandler={ techSpecMultiSelectChangeHandler }
        defaultTechSpecMultiSelection={defaultTechSpecMultiSelection}
        overrideGridRowData = {overrideGridRowData}
        overrideGridHeaderData = {overrideGridHeaderData}
        selectedMyTask={selectedMyTask}
        isDashboardARSView={isDashboardARSView}
        hasRejectedResources={hasRejectedResources}
        rejectedResourceValue={rejectedResourceValue}
        overrideTaskStatus={overrideTaskStatus}
        interactionMode={props.interactionMode}
        commentsHistoryClickHandler = { commentsHistoryClickHandler } />
        {/* {isARSSearch ? <Btn buttons={buttons} onClick={props.onClick}/> : null} */}
    </div>
  );
};
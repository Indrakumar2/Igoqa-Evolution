export const generateBudgetArray = (data) =>{
    const contractData = data.filter(item => item.budgetInformationType === "Contract");
   const projectData = data.filter(item => item.budgetInformationType === "Project");
    const assignmentData = data.filter(item => item.budgetInformationType === 'Assignment');
  
if (contractData.length > 0) {

    contractData.map((item, i) => {
       // delete contractData[i]['assignmentNumber'];
    });
}
if (projectData.length > 0) {

    projectData.map((item, i) => {
       // delete projectData[i]['assignmentNumber'];
        delete projectData[i]['contractNumber'];
        delete projectData[i]['customerContractNumber'];
    });
}
if (assignmentData.length > 0) {

    assignmentData.map((item, i) => {
        delete assignmentData[i]['projectNumber'];
        delete assignmentData[i]['contractNumber'];
        delete assignmentData[i]['customerContractNumber'];
    });
}
    projectData.map(x => {
        let tempAssignmentData = assignmentData.filter(item => item.projectId === x.projectId);

        if(tempAssignmentData.length > 0)
        {
            x["hasChild"] = "true";
            x["childArray"] = tempAssignmentData;
            x["groupName"] = `Project (${ x.projectNumber })`;
        }
        else{
            x["hasChild"] = "false";
        }
        tempAssignmentData = [];
    });
    contractData.map(x=>{

         let tempProjectData = projectData.filter(item => item.contractId === x.contractId);
        
        if(tempProjectData.length > 0)
        {
            x["hasChild"] = "true";
            x["childArray"] = tempProjectData;
            x["groupName"] = `Contract (${ x.contractNumber })`;
        }
        else{
            x["hasChild"] = "false";
        }
        tempProjectData = [];

    });  
 
return contractData;
};

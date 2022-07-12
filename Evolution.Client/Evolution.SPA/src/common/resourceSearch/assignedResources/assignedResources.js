import React, { Fragment } from 'react';
import { GridView,Btn } from '../resourceFields';
export const AssignedResource = (props) => {
  return (
          <div className= {props.isARSSearch ? "col s12 mb-3" : 'col s12 pl-0 pr-0' }>
              <h6 className="pl-0 bold"><span> Assigned Resource </span></h6>
          <GridView gridData={props.gridData} headerData={props.gridHeaderData}/>
          <Btn buttons={props.buttons}/>
          </div>
  
  );
};
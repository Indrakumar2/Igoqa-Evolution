import React, { Fragment } from 'react';
import { GridView,Btn } from '../resourceFields';
export const SubSupplierDetails = (props) => {
  return (
          <div className= {props.isARSSearch ? "col s12 mb-3" : 'col s12 pl-0 pr-0' }>
          <GridView gridData={props.gridData} headerData={props.gridHeaderData} gridRef={props.gridRef} rowClassRules = {props.rowClassRules}/>
          {!props.isARSSearch && <Btn buttons={props.buttons} interactionMode={props.interactionMode} />}
          </div>
  
  );
};
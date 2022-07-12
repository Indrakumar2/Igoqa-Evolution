import React,{ Component,Fragment } from 'react';

const errorList = (props) =>{
      const errors =(
        props.errors.map(value =>{
            return(
                <ul className='mb-2 mt-2'>
                     <li>{value}</li>
                </ul>
            );
        })
      );

    return (
      <Fragment>
          {errors}
      </Fragment>
    );
};

export default errorList;
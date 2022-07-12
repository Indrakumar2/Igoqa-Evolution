import pdfMake from 'pdfmake/build/pdfmake';
import pdfFonts from 'pdfmake/build/vfs_fonts';

pdfMake.vfs = pdfFonts.pdfMake.vfs;

export const generatePdf = ( docDefinition, filename) =>{
    pdfMake.createPdf( docDefinition ).download( filename );
};
// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ModalResult } from '@aksio/cratis-mui';
import { GridColDef } from '@mui/x-data-grid';
import { ValueEditorFor } from 'Components';
import { AddCertificateToApplicationEnvironment } from 'API/applications/environments/AddCertificateToApplicationEnvironment';
import { CertificatesForApplicationEnvironmentId } from 'API/applications/environments/CertificatesForApplicationEnvironmentId';
import { AddCertificateDialog, AdCertificateDialogOutput } from './AddCertificateDialog';
import { useRouteParams } from '../RouteParams';
import { Certificate } from 'API/applications/environments/Certificate';

const columns: GridColDef[] = [
    { field: 'name', headerName: 'Name', width: 250 }
];

export const Certificates = () => {
    const { applicationId, environmentId } = useRouteParams();
    const [certificatesForEnvironment] = CertificatesForApplicationEnvironmentId.use({ environmentId: environmentId! });
    const certificates = certificatesForEnvironment.data.certificates ?? [];

    console.log(certificates);

    return (
        <ValueEditorFor<AdCertificateDialogOutput, Certificate>
            addTitle="Add Certificate"
            columns={columns}
            data={certificates}
            modalContent={AddCertificateDialog}
            getRowId={(certificate) => certificate.name}
            modalClosed={async (result, output) => {
                if (result == ModalResult.success) {
                    const command = new AddCertificateToApplicationEnvironment();
                    command.applicationId = applicationId!;
                    command.environmentId = environmentId!;
                    command.name = output!.name;
                    command.certificate = output!.certificate;
                    await command.execute();
                }
            }} />
    );
};

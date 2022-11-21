// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ModalResult } from '@aksio/cratis-mui';
import { GridColDef } from '@mui/x-data-grid';
import { ValueEditorFor } from 'Components';
import { Ingress } from 'API/applications/environments/ingresses/Ingress';
import { AddRouteDialog, AddRouteDialogOutput, AddRouteDialogInput } from './AddRouteDialog';
import { IngressRoute } from 'API/applications/environments/ingresses/IngressRoute';
import { DefineRouteForIngress } from 'API/applications/environments/ingresses/DefineRouteForIngress';
import { useRouteParams } from '../../RouteParams';


export interface RoutesProps {
    ingress: Ingress;
}

const columns: GridColDef[] = [
    { field: 'path', headerName: 'Path', width: 250 },
    { field: 'targetPath', headerName: 'Target Path', width: 250 }
];

export const Routes = (props: RoutesProps) => {
    const input = useRouteParams() as AddRouteDialogInput;

    const routes = props.ingress.routes ?? [];

    return (
        <ValueEditorFor<AddRouteDialogOutput, IngressRoute, AddRouteDialogInput>
            addTitle="Add route"
            columns={columns}
            data={routes}
            modalContent={AddRouteDialog}
            input={input}
            getRowId={(route) => route.path}
            modalClosed={async (result, output) => {
                if (result == ModalResult.success) {
                    const command = new DefineRouteForIngress();
                    command.applicationId = input.applicationId;
                    command.environmentId = input.environmentId;
                    command.ingressId = input.ingressId;
                    command.path = output!.path;
                    command.targetPath = output!.targetPath;
                    command.targetMicroservice = output!.targetMicroservice;
                    await command.execute();
                }
            }} />
    );
};

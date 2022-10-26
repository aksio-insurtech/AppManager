// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useParams } from 'react-router-dom';

export interface RouteParams {
    applicationId: string;
    environmentId?: string;
    microserviceId?: string;
    deployableId?: string;
    ingressId?: string;
}

export const useRouteParams = () => {
    return useParams() as unknown as RouteParams;
};

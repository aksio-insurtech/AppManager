/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{applicationId}/microservices/{{microserviceId}}/stack/{{environment}}');

export interface ISetStack {
    microserviceId?: string;
}

export class SetStackValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        microserviceId: new Validator(),
    };
}

export class SetStack extends Command<ISetStack> implements ISetStack {
    readonly route: string = '/api/applications/{applicationId}/microservices/{{microserviceId}}/stack/{{environment}}';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new SetStackValidator();

    private _microserviceId!: string;

    get requestArguments(): string[] {
        return [
            'microserviceId',
            'environment',
        ];
    }

    get properties(): string[] {
        return [
            'microserviceId',
        ];
    }

    get microserviceId(): string {
        return this._microserviceId;
    }

    set microserviceId(value: string) {
        this._microserviceId = value;
        this.propertyChanged('microserviceId');
    }

    static use(initialValues?: ISetStack): [SetStack, SetCommandValues<ISetStack>] {
        return useCommand<SetStack, ISetStack>(SetStack, initialValues);
    }
}

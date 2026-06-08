export interface TableColumn {
    key: string;
    label: string;
    type?: 'text' | 'chip' | 'action';
    valueMap?: Record<string, { label: string; class: string }>;

    actions?: string[];
}
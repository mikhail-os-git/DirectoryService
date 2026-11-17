export interface ILinkInfo {
  name: string,
  path: string
}


export default interface NavBarProps extends React.ComponentProps<'nav'> {
  links: ILinkInfo[];
}

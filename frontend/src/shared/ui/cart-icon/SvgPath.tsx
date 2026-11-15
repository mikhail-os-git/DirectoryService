import { CartIconProps } from './CartIcon';


export function SvgPath({ paths }: CartIconProps) {
  return <svg aria-hidden="true" fill="none" viewBox="0 0 24 24" width={20} height={20}>
    {paths.map((d) => (
      <path
        key={d}
        stroke="currentColor"
        strokeLinecap="round"
        strokeLinejoin="round"
        strokeWidth={2}
        d={d} />
    ))}
  </svg>;
}

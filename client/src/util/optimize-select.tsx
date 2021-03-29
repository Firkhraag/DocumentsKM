import { MenuListComponentProps } from 'react-select/src/components/Menu'
import { FixedSizeList } from 'react-window'
import { OptionProps } from 'react-select/src/components/Option'
import { components } from 'react-select'
import React from 'react'

export const optimizeSelect = {
  components: {
    MenuList: OptimizedMenuList,
    Option: OptimizedOption,
  },
}

function OptimizedMenuList(props: MenuListComponentProps<SelectOption, boolean>) {
  const { options, children, maxHeight, getValue } = props
  if (!children || !Array.isArray(children)) return null

  const height = 35
  const selectedValues = getValue() as SelectOption[]
  const initialOffset = selectedValues[0] ? options.indexOf(selectedValues[0]) * height : 0

  return (
    <FixedSizeList
      width={''}
      itemSize={height}
      height={maxHeight}
      itemCount={children.length}
      initialScrollOffset={initialOffset}
    >
      {({ index, style }) => (
        <div className="option-wrapper" style={style}>
          {children[index]}
        </div>
      )}
    </FixedSizeList>
  )
}

function OptimizedOption(props: OptionProps<SelectOption, boolean>) {
  delete props.innerProps.onMouseMove
  delete props.innerProps.onMouseOver
  return <components.Option {...props}>{props.children}</components.Option>
}

// SelectOption is specific to this example
// and may not work with other projects
type SelectOption = {
  value: string
  label: string
  [key: string]: string
}
